using System;
using System.Collections.Generic;

namespace KieliOppija
{
    internal class TextAnalyzer
    {
        internal static List<string> AnalyzeWords(List<AnalyzedSentence> sentences)
        {
            List<string> lista = new List<string>();
                        
            foreach (AnalyzedSentence s in sentences)
            {
                lista.AddRange(s.Content.Split(new char[] { ' ', '\n' }));
            }
            return lista;
        }

        /// <summary>
        /// Analyze one sentence
        /// </summary>
        /// <param name="sentence">Sentence</param>
        /// <returns>Array of word strings</returns>
        internal static List<string> AnalyzeWords(AnalyzedSentence sentence)
        {
            List<string> lista = new List<string>();

            //foreach (AnalyzedSentence s in sentences)
            {
                lista.AddRange(sentence.Content.Split(new char[] { ' ', '\n' }));
            }
            return lista;
        }

        internal static List<AnalyzedSentence> AnalyzeSentences(string text)
        {
            List<AnalyzedSentence> lista = new List<AnalyzedSentence>();
            string[] contents= text.Split(new char[] { '.'});     
            foreach(string s in contents)
            {
                string s2=s.Trim();
                if (s2.Length == 0) continue;
                string[] parts = s2.Split(new char[] { ',' });
                
                foreach (string part in parts)
                {
                    lista.Add(AnalyzeSentenceParts(part.Trim()));
                }
                
            }
            return lista;
        }

        private static AnalyzedSentence AnalyzeSentenceParts(string part)
        {
            string[] keywords = {"että","jotta","koska","kun","jos","vaikka","kuin" };
            if (part.EndsWith(".")) {
                part = part.Substring(0, part.Length - 1);
            }
            foreach (string k in keywords)
            {
                if (part.StartsWith(k)) return new AnalyzedSentence(part, false);
            }
            return new AnalyzedSentence(part, true);
        }
    }
}