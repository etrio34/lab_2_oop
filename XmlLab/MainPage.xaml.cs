using System.Xml.Xsl;

namespace XmlLab;

public partial class MainPage : ContentPage
{
    private const string AnyValue = "(будь-який)";

    private string xmlPath = "";
    private AnalysisContext context = new AnalysisContext(new SaxAnalyzer());
    private XslTransformer transformer = new XslTransformer();

    public MainPage()
    {
        InitializeComponent();
        StrategyPicker.SelectedIndex = 0;
    }

    private void OnStrategyChanged(object sender, EventArgs e)
    {
        if (StrategyPicker.SelectedIndex == 0)
        {
            context.SetAnalyzer(new SaxAnalyzer());
        }
        else if (StrategyPicker.SelectedIndex == 1)
        {
            context.SetAnalyzer(new DomAnalyzer());
        }
        else if (StrategyPicker.SelectedIndex == 2)
        {
            context.SetAnalyzer(new LinqAnalyzer());
        }
    }

    private async void OnChooseFileClicked(object sender, EventArgs e)
    {
        FileResult? file = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Оберіть XML-файл"
        });
        if (file == null)
        {
            return;
        }
        if (!file.FileName.EndsWith(".xml"))
        {
            await DisplayAlert("Помилка", "Потрібен файл з розширенням .xml", "OK");
            return;
        }

        try
        {
            FillPicker(AuthorPicker, context.GetValues(file.FullPath, "Author"));
            FillPicker(TypePicker, context.GetValues(file.FullPath, "Type"));
            xmlPath = file.FullPath;
            FileLabel.Text = file.FileName;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Помилка", ex.Message, "OK");
        }
    }

    private async void OnSearchClicked(object sender, EventArgs e)
    {
        if (xmlPath == "")
        {
            await DisplayAlert("Помилка", "Спочатку оберіть XML-файл", "OK");
            return;
        }

        try
        {
            List<Article> articles = context.Search(xmlPath, GetCriteria());

            string text = "Знайдено статей: " + articles.Count + "\n\n";
            foreach (Article article in articles)
            {
                text += article.ToString() + "\n";
            }
            ResultEditor.Text = text;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Помилка", ex.Message, "OK");
        }
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        TitleEntry.Text = "";
        AnnotationEntry.Text = "";
        if (AuthorPicker.Items.Count > 0)
        {
            AuthorPicker.SelectedIndex = 0;
        }
        if (TypePicker.Items.Count > 0)
        {
            TypePicker.SelectedIndex = 0;
        }
        ResultEditor.Text = "";
    }

    private async void OnTransformClicked(object sender, EventArgs e)
    {
        if (xmlPath == "")
        {
            await DisplayAlert("Помилка", "Спочатку оберіть XML-файл", "OK");
            return;
        }

        try
        {
            string htmlPath = Path.ChangeExtension(xmlPath, ".html");
            await transformer.TransformAsync(xmlPath, htmlPath);
            await DisplayAlert("Готово", "HTML-файл збережено: " + htmlPath, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Помилка", ex.Message, "OK");
        }
    }

    private async void OnExitClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Вихід", "Чи дійсно ви хочете завершити роботу з програмою?", "Так", "Ні");
        if (answer)
        {
            Application.Current?.Quit();
        }
    }

    private Dictionary<string, string> GetCriteria()
    {
        Dictionary<string, string> criteria = new Dictionary<string, string>();

        if (!string.IsNullOrWhiteSpace(TitleEntry.Text))
        {
            criteria["Title"] = TitleEntry.Text.Trim();
        }
        if (!string.IsNullOrWhiteSpace(AnnotationEntry.Text))
        {
            criteria["Annotation"] = AnnotationEntry.Text.Trim();
        }
        if (AuthorPicker.SelectedIndex > 0)
        {
            criteria["Author"] = AuthorPicker.Items[AuthorPicker.SelectedIndex];
        }
        if (TypePicker.SelectedIndex > 0)
        {
            criteria["Type"] = TypePicker.Items[TypePicker.SelectedIndex];
        }
        return criteria;
    }

    private void FillPicker(Picker picker, List<string> values)
    {
        List<string> items = new List<string>();
        items.Add(AnyValue);
        items.AddRange(values);
        picker.ItemsSource = items;
        picker.SelectedIndex = 0;
    }
}