using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StravaGPX
{
    class Parser
    {
        private Queue<String> _links;
        private LinkDict _dict;

        public Parser(String link, int max_queue = 1024)
        {
            _links = new Queue<string>();
            _dict = new LinkDict(max_queue);
            _links.Enqueue(link);
            _dict.Add(link);

        }
        // добавление  списка в очередь
        // возврат числа добавленных элементов
        public int AddList(List<String> list)
        {
            int count = 0;
            foreach (string link in list)
            {
                
                if (_dict.Add(link))
                {
                    _links.Enqueue(link);
                    count++;
                }
            }
            return count;
        }

        public String GetLink()
        {
            return _links.Dequeue();
        }

        public int GetVolueDict(){
            return _dict.Count();
        }

        public int StackSize(){
            return _links.Count;
        }

        public List<string> GetDict(){
            return _dict.GetDict();
        }

        private class LinkDict
        {
            private List<string> _dict;
            private int max_queue;
            public LinkDict(int max_queue){
                _dict = new List<string>();
                this.max_queue = max_queue;
            }
            
            public bool Add(String link)
            {
                if (_dict.Contains(link)||_dict.Count>=max_queue)
                {
                    return false;
                    
                }
                _dict.Add(link);
                    return true;
            }
            public bool Contains(string link){
                return _dict.Contains(link);
            }
            public int Count(){
                return _dict.Count;
            }
            public List<string> GetDict(){
                return _dict;
            }
        }
    }
}