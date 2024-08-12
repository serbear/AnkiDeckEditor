using System.Collections.ObjectModel;
using System.Linq;
using AnkiDeckEditor.Models;

namespace AnkiDeckEditor.Services;

public static class CollectionLoader
{
    public static ObservableCollection<ToggleItem> LoadVerbControls()
    {
        const string collection =
            "et\nkas\nkeda\nkeda + millest\nkeda + mis\nkelle eest\nkelle juurde\nkelle juures\nkelle järele\nkelle käest\nkelle käest + mida\nkelle otsa\nkelle peale\nkelle pärast\nkelle tõttu\nkelle vastu\nkellega\nkellega + milles\nkelleks\nkellel + mida teha\nkellele\nkellele + mida\nkellelt\nkellelt käest\nkellesse\nkellest\nkelleta\nkuhu\nkuhu + millega\nkui palju\nkuidas\nkus\nkust\nkust + kuhu\nmida\nmida tegema\nmida tegemas\nmida tegemast\nmida tegevat\nmida teha\nmidagi tegemata\nmille all\nmille alla\nmille eest\nmille järele\nmille järgi\nmille kohta\nmille käes\nmille otsa\nmille peale\nmille poolest\nmille pärast\nmille tõttu\nmille vastu\nmille üle\nmillega\nmilleks\nmillele\nmilles\nmillesse\nmillest\nmilleta\nmilline\nmilliseks\nmillisena\nmis keelde\nmis keelest\nmissuguseks";
        var collectionArray = collection.Split("\n");
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
            new SpeechPartToggleItem("глагол", "tegusõna", true, false)
        ];
        return output;
    }
}

public struct FieldTags
{
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

    /// <summary>
    /// 1 - ains nim<br/>
    /// 2 - ains om<br/>
    /// 3 - ains os<br/>
    /// 4 - mitm nim<br/>
    /// 5 - mitm om<br/>
    /// 6 - mitm os<br/>
    /// 7 - sse <br/>
    /// </summary>
    public const string SimpleWordItemsTemplate =
        "<div class=\"grid-item amount\">AIN.</div>\n" +
        "<div class=\"grid-item form\">{1}</div>\n" +
        "<div class=\"grid-item form\">{2}</div>\n" +
        "<div class=\"grid-item form\">{3}</div>\n" +
        "<div class=\"grid-item amount\">MIT.</div>\n" +
        "<div class=\"grid-item form\">{4}</div>\n" +
        "<div class=\"grid-item form\">{5}</div>\n" +
        "<div class=\"grid-item form\">{6}</div>\n" +
        "<div class=\"grid-item short-into\">L.SSE.</div>\n" +
        "<div class=\"grid-item form-short-into\">{7}</div>";

    public const string VerbTemplate = "<div class=\"word-forms-container\">{1}</div>";

    /// <summary>
    /// 1 - ma<br/>
    /// 2 - 3p.min<br/>
    /// 3 - da<br/>
    /// 4 - nud<br/>
    /// 5 - 3p<br/>
    /// 6 - 2p.kas<br/>
    /// 7 - tud<br/>
    /// 8 - akse<br/>
    /// </summary>
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
}