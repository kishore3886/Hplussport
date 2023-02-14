namespace Hplussport.API.Classes
{
    public class QueryParameters
    {

       const int _maxSize = 1000;

       private int _size = 50;
        
       public int Page { get; set; }

        public int Size
        {

            get 
            {
                return _size;
            }
            set 
            { 
                _size=Math.Min(value, _maxSize); 
            }    
        }

        public string SortBy { get; set; } = "Id";
        public string _SortOrder { get; set; } = "asc";

        public string SortOrder {
            get
            {

                return _SortOrder;
            }
            set 
            {
                if(value == "asc" || value == "desc")
                {
                    _SortOrder=value;
                }                             
            }
        }

    }
}
