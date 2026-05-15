namespace RestauranteApi.Entidades
{
    public class Zona
    {
        public int Id { get; set; }
       
        public string Nombre { get; set; } = string.Empty;
        
        public string Descripcion { get; set; } = string.Empty;
      

        public List<Mesa> Mesas { get; set; } = new();
        
    }
}