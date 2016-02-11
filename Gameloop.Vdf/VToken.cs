namespace Gameloop.Vdf
{
    public abstract class VToken
    {
        public VToken Parent { get; set; }
        public VToken Previous { get; set; }
        public VToken Next { get; set; }
    }
}
