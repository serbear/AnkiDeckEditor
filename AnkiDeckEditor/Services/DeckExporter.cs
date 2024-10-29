using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using AnkiDeckEditor.Enums;
using AnkiDeckEditor.Models;
using AnkiDeckEditor.Services.FieldsCopy;

namespace AnkiDeckEditor.Services;

// 1 line - separator.

// apple;banana;grape
// some text;other text;yet more text

//Because quotes are used to mark where a field begins and ends, if you wish to include them inside your field, you need to replace a single doublequote with two doublequotes to "escape" them from the regular handling, like so:
// 
// field one;"field two with ""escaped quotes"" inside it"

// You need to turn on the "allow HTML in fields" checkbox in the import dialog for HTML newlines to work. 

// If you wish to use HTML for formatting your file but also wish to include angle brackets or ampersands, you may use the following replacements:
// Character	Replacement
// <	&lt;
// >	&gt;
// &	&amp;
public static class DeckExporter
{
    public static void ExportCollection(
        ObservableCollection<EstonianCardRecord> cardCollectionItems,
        Dictionary<StrategyNames, string?> copyStrategyDict)
    {
        var sb = new StringBuilder();

        foreach (var card in cardCollectionItems)
        {
            GetContext(card);
            var copyStrategyDataDict = new Dictionary<StrategyNames, object>
            {
                { StrategyNames.LiteralTranslation, LiteralTranslationContext },
                { StrategyNames.LiteraryTranslation, LiteraryTranslationContext },
                { StrategyNames.OriginalText, OriginalTextContext },
                { StrategyNames.SpeechPart, card.SpeechPart },
                { StrategyNames.SpeechPartGovernment, card.SpeechPartGovernment },
                { StrategyNames.NonVerbWordForms, GetNonVerbWordForms(card) },
                { StrategyNames.VerbWordForms, GetVerbWordForms(card) }
            };

            
            // todo: Если не глагол, пропускать глагольные формы слова.
            
            foreach (var copyStrategy in Enum.GetValues(typeof(StrategyNames)))
            {
                var copyStrategyValue = (StrategyNames)copyStrategy;
                var strategy = Activator.CreateInstance(Type.GetType(copyStrategyDict[copyStrategyValue]!)!);
                var copyContext = new Context();
                copyContext.SetStrategy((strategy as ICopyStrategy)!);


                var isDataCollectionExist = copyStrategyDataDict.TryGetValue(copyStrategyValue, out var fieldValue);


                // Если поле пустое, вставить пустое значение.


                object result;


                if (!isDataCollectionExist)
                    result = FieldHelper.GetFieldValue(card, copyStrategyValue.ClassFieldName());
                else
                    result = fieldValue is Func<NonVerbWordFormCollection> func
                        ? func()
                        : fieldValue!;

                copyContext.DoCopyLogic(result, out var fieldText);

                Console.WriteLine($"{copyStrategy} === {fieldText}");
            }
        }
    }


    private static (string, List<int>) LiteralTranslationContext { get; set; }
    private static (string, List<int>) LiteraryTranslationContext { get; set; }
    private static (string, List<int>) OriginalTextContext { get; set; }

    private static void GetContext(EstonianCardRecord card)
    {
        LiteralTranslationContext = (card.LiteralTranslationText, card.LiteralTranslationSelection);
        LiteraryTranslationContext = (card.LiteraryTranslationText, card.LiteraryTranslationSelection);
        OriginalTextContext = (card.OriginalText, card.OriginalTextSelection);
    }

    private static NonVerbWordFormCollection GetNonVerbWordForms(EstonianCardRecord card)
    {
        // todo: refacf: auto

        return new NonVerbWordFormCollection(
            [
                card.NominativeCaseSingularWordForm,
                card.GenitiveCaseSingularWordForm,
                card.PartitiveCaseSingularWordForm,
                card.NominativeCasePluralWordForm,
                card.GenitiveCasePluralWordForm,
                card.PartitiveCasePluralWordForm,
                card.ShortIllativeCaseWordForm
            ]
        );
    }

    private static VerbWordFormCollection GetVerbWordForms(EstonianCardRecord card)
    {
        // todo: refacf: auto

        return new VerbWordFormCollection(
        [
            card.MaInfinitiveWordForm,
            card.DaInfinitiveWordForm,
            card.IndicativeMoodWordForm,
            card.PassiveParticiplePastTenseWordForm,
            card.ThirdPersonPastTenseWordForm,
            card.ActiveParticipleWordForm,
            card.ImperativeMoodSingularWordForm,
            card.PassiveVoicePresentTenseWordForm
        ]);
    }
}