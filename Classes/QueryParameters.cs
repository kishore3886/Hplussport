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

    }
}
