using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using AnkiDeckEditor.Enums;
using AnkiDeckEditor.Models;
using AnkiDeckEditor.Services.FieldsCopy;


namespace AnkiDeckEditor.Services;

public static class DeckExporter
{
    private static (string, List<int>) LiteralTranslationContext { get; set; }
    private static (string, List<int>) LiteraryTranslationContext { get; set; }
    private static (string, List<int>) OriginalTextContext { get; set; }

    public static bool ExportCollection(ObservableCollection<EstonianCardRecord> cardCollectionItems,
        Dictionary<StrategyNames, string?> copyStrategyDict, string collectionFileName)
    {
        var sb = PrepareFileContent(cardCollectionItems, copyStrategyDict);
        return SaveFile(sb, collectionFileName);
    }

    private static StringBuilder PrepareFileContent(
        ObservableCollection<EstonianCardRecord> cardCollectionItems,
        IReadOnlyDictionary<StrategyNames, string?> copyStrategyDict)
    {
        var sb = new StringBuilder();
        const char CARD_SEPARATOR = '\t';

        AddFileHeader(ref sb);

        foreach (var card in cardCollectionItems)
        {
            var copyStrategyDataDict = PrepareStrategies(card);

            foreach (var copyStrategy in Enum.GetValues(typeof(StrategyNames)))
            {
                if (!copyStrategyDataDict.ContainsKey((StrategyNames)copyStrategy)) continue;

                // Select a copy strategy
                var strategy = SelectStrategy(copyStrategy, copyStrategyDict);
                if (strategy == null) continue;

                // Forming a data context being copied.

                var copyContext = new Context();
                copyContext.SetStrategy(strategy.Item2);

                var isDataCollectionExist = copyStrategyDataDict.TryGetValue(strategy.Item1, out var fieldValue);

                // todo: ??? Если поле пустое, вставить пустое значение.

                object result;

                if (!isDataCollectionExist)
                    result = FieldHelper.GetFieldValue(card, strategy.Item1.ClassFieldName());
                else
                    result = fieldValue is Func<NonVerbWordFormCollection> func
                        ? func()
                        : fieldValue!;

                copyContext.DoCopyLogic(result, out var fieldText);

                // Prepare the file string.

                fieldText = fieldText.Replace("\"", "\"\"");
                sb.Append($"\"{fieldText}\"");
                sb.Append(CARD_SEPARATOR);
            }

            sb.Append('\n');
        }

        return sb;
    }

    private static bool SaveFile(StringBuilder sb, string collectionFileName)
    {
        // todo: settings: add or rewrite.

        var result = true;

        try
        {
            // todo: file write permissions

            File.WriteAllText(collectionFileName, sb.ToString());
        }
        catch (Exception ex)
        {
            result = false;
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return result;
    }

    private static void AddFileHeader(ref StringBuilder builder)
    {
        const string SEPARATOR_NAME = "tab";
        builder.Append($"#separator:{SEPARATOR_NAME}\n");
        builder.Append("#html:true\n");
    }

    private static Dictionary<StrategyNames, object> PrepareStrategies(EstonianCardRecord card)
    {
        GetContext(card);

        var result = new Dictionary<StrategyNames, object>
        {
            { StrategyNames.LiteralTranslation, LiteralTranslationContext },
            { StrategyNames.LiteraryTranslation, LiteraryTranslationContext },
            { StrategyNames.OriginalText, OriginalTextContext },
            { StrategyNames.SpeechPart, card.SpeechPart! },
            { StrategyNames.SpeechPartGovernment, card.SpeechPartGovernment },
            { StrategyNames.NonVerbWordForms, GetNonVerbWordForms(card) },
            { StrategyNames.VerbWordForms, GetVerbWordForms(card) },
            { StrategyNames.VocabularyEntry, card.VocabularyEntryText }
        };

        // Если не глагол, пропускать глагольные формы слова.
        result.Remove(
            card.SpeechPart!.VerbType == null
                ? StrategyNames.VerbWordForms
                : StrategyNames.NonVerbWordForms);
        return result;
    }

    private static Tuple<StrategyNames, ICopyStrategy?>? SelectStrategy(
        object copyStrategy,
        IReadOnlyDictionary<StrategyNames, string?> copyStrategyDict)
    {
        var copyStrategyValue = (StrategyNames)copyStrategy;
        object strategy;
        try
        {
            strategy = Activator.CreateInstance(Type.GetType(copyStrategyDict[copyStrategyValue]!)!)!;
        }
        catch (KeyNotFoundException)
        {
            return null;
        }

        return new Tuple<StrategyNames, ICopyStrategy?>(copyStrategyValue, strategy as ICopyStrategy);
    }

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