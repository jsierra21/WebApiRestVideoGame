namespace Core.Messages
{
    public static class ErrorMessage
    {
        public static string NullError = "El campo {PropertyName} no puede ser nulo";
        public static string LengthError = "La longitud del campo {PropertyName} debe estar entre {MinLength} y {MaxLength} caractéres. Se han encontrado {TotalLength} caractéres.";
        public static string EmptyError = "El campo {PropertyName} no puede estar vacío";
        public static string MatchError = "El formato del campo {PropertyName} no corresponde";
        public static string ValueError = "Valor de {PropertyName} inválido";
        public static string ValueWithValueError = "Valor de {PropertyName} inválido. Valor: {PropertyValue}";
        public static string CountError = "Debe haber al menos 1 {PropertyName}";
        public static string PasswordError = "Las contraseñas no coinciden";
        public static string TelError = "El {PropertyName} debe tener un máximo de 10 dígitos.";
        public static string DecimalError = "'{PropertyName}' no debe tener más de {ExpectedPrecision} dígitos en total, con margen para {ExpectedScale} decimales. Se encontraron {Digits} dígitos y {ActualScale} decimales ({PropertyValue}).";
        public static string MaxLengthError = "La longitud de '{PropertyName}' debe tener {MaxLength} caractéres o menos. Total ingresados: {TotalLength}.";
    }
}
