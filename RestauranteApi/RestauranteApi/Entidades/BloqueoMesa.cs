namespace RestauranteApi.Entidades
{
    public class BloqueoMesa
    {
        private Mesa? mesa;

        public int Id { get; set; }

        public DateTime Inicio { get; set; }

        public DateTime Fin { get; set; }

        public string Motivo { get; set; } = string.Empty;

        public int MesaId { get; set; }
        public Mesa? Mesa { get => mesa; set => mesa = value; }


    }
}
