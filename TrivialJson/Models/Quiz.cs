using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using TrivialJson.Models;

namespace TrivialJson.Models
{
    public class Quiz
    {
        public int Punteggio {get; private set;}
        public string APIUrl { get; private set; }
        public List<Domanda> ListaDomande { get; private set; }

        public Dictionary<string, string> DomandaRisposta { get; private set; }

        public Quiz(string apiurl)
        {
            APIUrl = apiurl;
            ListaDomande = new List<Domanda>();
            DomandaRisposta = new Dictionary<string, string>();
            ImportaDomande();
            ImpostaCoppie();
            Punteggio = 0;
        }

        private void ImportaDomande()
        {
            string url = APIUrl;

            HttpClient httpClient = new HttpClient();

            string risposta = httpClient.GetStringAsync(url).Result;

            Root r = JsonSerializer.Deserialize<Root>(risposta);
            ListaDomande = r.results;
        }

        private void ImpostaCoppie()
        {
            StringBuilder sb;
            string dom;
            string risposta;
            int answerindex;
            bool[] appoggio;
            Random rd = new Random();
            foreach (Domanda domanda in ListaDomande)
            {
                appoggio = new bool[4];

                sb = new StringBuilder();
                sb.AppendLine(domanda.question);
                for (int i = 0; i < appoggio.Length; i++)
                {
                    answerindex = rd.Next(0, 4);
                    while (appoggio[answerindex])
                    {
                        answerindex = rd.Next(0, 4);
                    }
                    switch (answerindex)
                    {
                        case 0:
                            sb.AppendLine($"{i = 1}. {domanda.correct_answer}");
                            break;
                        case 1:
                            sb.AppendLine($"{i = 1}. {domanda.incorrect_answers[0]}");
                            break;
                        case 2:
                            sb.AppendLine($"{i = 1}. {domanda.incorrect_answers[1]}");
                            break;
                        case 3:
                            sb.AppendLine($"{i = 1}. {domanda.incorrect_answers[2]}");
                            break;
                    }
                }
                dom = sb.ToString();
                risposta = domanda.correct_answer;
                DomandaRisposta.Add(dom, risposta);

            }
        }

        public string GetDomanda()
        {
            Random rd = new Random();
            int indexdom = rd.Next(0, DomandaRisposta.Count);
            return DomandaRisposta[ListaDomande[indexdom].question];
        }

        public void Rispondi(string domanda,  string risposta)
        {
        
            foreach(var d in ListaDomande) 
            {
                if(d.question == domanda)
                { 
                    if(risposta == d.correct_answer)
                    {
                        Punteggio++;
                    }
                }
            }
        }
    }
}
