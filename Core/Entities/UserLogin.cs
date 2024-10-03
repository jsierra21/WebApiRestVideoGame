namespace Core.Entities
{
    public class UserLogin : BaseEntity
    {
        public string Correo { get; set; }
        public string Password { get; set; }
    }

    public partial class AccountLogin
    {
        public string Correo { get; set; }
        public string Password { get; set; }
    }

    public partial class AccountPerfil
    {
        public string Usuario { get; set; }
        public int EmpresaProceso { get; set; }
        public int Perfil { get; set; }
    }

    public partial class AccountPerfilMenu
    {
        public string Usuario { get; set; }
        public int EmpresaProceso { get; set; }
        public string Perfil { get; set; }
    }

    public partial class AccountPermisos
    {
        public string Usuario { get; set; }
        public string Programa { get; set; }
        public int Modulo { get; set; }
        public int Perfil { get; set; }
    }



}
