using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StravaGPX{
    class Parser {
        private Queue<String> _links;
        private int _max_queue = 2^16;
        public Parser(String link,int max_queue){
            _links = new Queue<string>();
            _links.Enqueue(link);
        }
        // добавление  списка в очередь
        // возврат числа добавленных элементов
        public int AddList(List<String> list){
            int count =0;
            foreach(string link in list){
                if(!_links.Contains(link)&&_links.Count<_max_queue){
                    _links.Enqueue(link);
                    count++;
                }
            }
            return count;
        }

        public String GetLink(){
            return _links.Dequeue();
        }
    }
}