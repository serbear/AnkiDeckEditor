using System.Collections.ObjectModel;
using System.Linq;
using AnkiDeckEditor.Models;

namespace AnkiDeckEditor.Services;

public static class CollectionLoader
{
    public static ObservableCollection<ToggleItem> LoadVerbControls()
    {
        const string COLLECTION =
            "et\nkas\nkeda\nkeda + millest\nkeda + mis\nkelle eest\nkelle juurde\nkelle juures\nkelle järele\nkelle käest\nkelle käest + mida\nkelle otsa\nkelle peale\nkelle pärast\nkelle tõttu\nkelle vastu\nkellega\nkellega + milles\nkelleks\nkellel + mida teha\nkellele\nkellele + mida\nkellelt\nkellelt käest\nkellesse\nkellest\nkelleta\nkuhu\nkuhu + millega\nkui palju\nkuidas\nkus\nkust\nkust + kuhu\nmida\nmida tegema\nmida tegemas\nmida tegemast\nmida tegevat\nmida teha\nmidagi tegemata\nmille all\nmille alla\nmille eest\nmille järele\nmille järgi\nmille kohta\nmille käes\nmille otsa\nmille peale\nmille poolest\nmille pärast\nmille tõttu\nmille vastu\nmille üle\nmillega\nmilleks\nmillele\nmilles\nmillesse\nmillest\nmilleta\nmilline\nmilliseks\nmillisena\nmis keelde\nmis keelest\nmissuguseks";
        var collectionArray = COLLECTION.Split("\n");
        var output = new ObservableCollection<ToggleItem>(
            collectionArray.Select(
                vc => new ToggleItem(vc, false)).ToList());
        return output;
    }

    public static ObservableCollection<SpeechPartToggleItem> LoadSpeechParts()
    {
        ObservableCollection<SpeechPartToggleItem> output =
        [
            new SpeechPartToggleItem("существительное", "nimisõna", false),
            new SpeechPartToggleItem("наречие", "määrsõna", false),
            new SpeechPartToggleItem("прилагательное", "omadussõna", false),
            new SpeechPartToggleItem("местоимение", "asesõna", false),
            // new SpeechPartToggleItem("глагол", "tegusõna", true, false)
            new SpeechPartToggleItem("глагол", "tegusõna", VerbTypes.Simple, false),
            new SpeechPartToggleItem("глагол составной", "ühendtegusõna", VerbTypes.Compound, false)
        ];
        return output;
    }
}

public struct FieldTags
{
    public const string CompoundVerbMarker = "<sup>üv</sup>";
    public const string TranslationOriginalTemplate = "<div class=\"sentence\">{1}</div>";

    public const string SelectedEntityTemplate = "<span>{1}</span>";

    /// <summary>
    /// 1 - russian translation; 2 - estonian translation
    /// </summary>
    public const string SpeechPartTemplate = "{1} <span>▪️</span> {2}";

    /// <summary>
    /// 1 - items
    /// </summary>
    public const string SimpleWordTemplate = "<div class=\"word-forms-container\">{1}</div>";

    public const string SimpleWordItemsWithSseFormTemplate =
        """
        <div class="grid-item amount">AIN.</div>
        <div class="grid-item form">{1}</div>
        <div class="grid-item form">{2}</div>
        <div class="grid-item form">{3}</div>
        <div class="grid-item amount">MIT.</div>
        <div class="grid-item form">{4}</div>
        <div class="grid-item form">{5}</div>
        <div class="grid-item form">{6}</div>
        <div class="grid-item short-into">L.SSE.</div>
        <div class="grid-item form-short-into">{7}</div>
        """;

    public const string VerbTemplate = "<div class=\"word-forms-container\">{1}</div>";

    public const string VerbItemsTemplate =
        "<div class=\"grid-item declension\">MA</div>\n" +
        "<div class=\"grid-item form\">{1}</div>\n" +
        "<div class=\"grid-item declension\">3P.MIN</div>\n" +
        "<div class=\"grid-item form\">{5}</div>\n" +
        "<div class=\"grid-item declension\">DA</div>\n" +
        "<div class=\"grid-item form\">{2}</div>\n" +
        "<div class=\"grid-item declension\">NUD</div>\n" +
        "<div class=\"grid-item form\">{6}</div>\n" +
        "<div class=\"grid-item declension\">3P</div>\n" +
        "<div class=\"grid-item form\">{3}</div>\n" +
        "<div class=\"grid-item declension\">2P.KÄS</div>\n" +
        "<div class=\"grid-item form\">{7}</div>\n" +
        "<div class=\"grid-item declension\">TUD</div>\n" +
        "<div class=\"grid-item form\">{4}</div>\n" +
        "<div class=\"grid-item declension\">AKSE</div>\n" +
        "<div class=\"grid-item form\">{8}</div>";

    public const string VerbControlTemplate = "<div class=\"verb-control-container\">{1}</div>";

    public const string VerbControlItemTemplate = "<span>{1}</span>";

    /// <summary>
    /// Returns the string representation of the replacement marker value in the templates with the location number.
    /// </summary>
    /// <param name="number">The place number.</param>
    public static string GetPlaceMarker(int number)
    {
        var output = $"{{{number}}}";
        return output;
    }
}