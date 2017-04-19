using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace KieliOppija
{
    internal class Dao
    {
        MySqlConnection conn=null;
        
        public Dao()
        {
            string connStr = "server=localhost;user=root;database=sanat;port=3306;password=;";
            conn = new MySqlConnection(connStr);
        }

        internal void saveSentences(List<AnalyzedSentence> sentences)
        {

            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "";
                foreach (AnalyzedSentence sent in sentences)
                {
                    int sentid = -1;
                    sentid= readWordOrSentence("lauseet", sent.Content);
                    if(sentid== -1) // not found
                    { 
                        sql = String.Format("INSERT INTO lauseet (lause, päälause) VALUES ('{0}','{1}')", sent.Content, sent.IsMain);
                        MySqlCommand cmd1 = new MySqlCommand(sql, conn);
                        cmd1.ExecuteNonQuery();
                        // kysy tämän sentencen id
                    }
                    sentid =readWordOrSentence("lauseet",sent.Content);

                    List<string> words = TextAnalyzer.AnalyzeWords(sent);
                    int len = words.Count;
                    for (int i = 0; i < len; i++)
                    {
                        int wordid = -1;
                        // if not already in table
                        wordid = readWordOrSentence("sanalista", words[i]);
                        if (wordid == -1)
                        {
                            sql = String.Format("INSERT INTO sanalista (sana_alkup) VALUES ('{0}')", words[i]);
                            MySqlCommand cmd1 = new MySqlCommand(sql, conn);
                            cmd1.ExecuteNonQuery();
                        }
                        // kysy tämän sanan id
                        wordid = readWordOrSentence("sanalista", words[i]);
                        Console.WriteLine(words[i] + ":" + wordid);
                        // table matches words to sentences
                        //wordid = readWordOrSentence("sana_lause", words[i]);
                        sql = String.Format("INSERT INTO sana_lause (sana,lause,järjestys_alusta,järjestys_lopusta)" +
                             " VALUES ({0},{1},{2},{3})", wordid, sentid, i+1, len - i);
                        try
                        {
                            MySqlCommand cmd = new MySqlCommand(sql, conn);
                            cmd.ExecuteNonQuery();
                        }
                        catch (InvalidOperationException ex)  { Console.WriteLine(ex.Message); }
                        catch (MySqlException ex) {             Console.WriteLine(ex.ToString()); }
                    }
                }
            }
            catch (InvalidOperationException ex) { Console.WriteLine(ex.Message); }
            catch (MySqlException ex)
            {                
                foreach (AnalyzedSentence sent in sentences)
                {
                    Console.WriteLine(sent);
                }
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
        }

        /// <summary>
        /// Read id. This method should be private because of table name in param.
        /// </summary>
        /// <param name="key">filter</param>
        /// <param name="table">database table</param>
        /// <returns>-1 if not found, id otherwise</returns>
        private int readWordOrSentence(string table, string key)
        {
            string sql = "";
            int sentid = -1;
            string col = table.CompareTo("lauseet") == 0 ? "lause" : "sana_alkup";
            sql = String.Format("SELECT id FROM {0} WHERE {1}='{2}'", table, col, key);

            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        sentid = rdr.GetInt32(0);
                        //while (rdr.Read()) { Console.WriteLine(rdr[0] + " -- " + rdr[1]);  }
                        //rdr.Close();
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return sentid;
        }

        private void saveWords(string[] words)
        {
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql="";
                foreach (string word in words)
                {
                    String.Format("INSERT INTO sanalista (sana_alkup) VALUES ('{0}')", word);
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (MySqlException ex)
            {
                foreach (string word in words)
                {
                    Console.WriteLine(word);
                }
                
                Console.WriteLine(ex.Message);
            }

            conn.Close();
        }
    }
}