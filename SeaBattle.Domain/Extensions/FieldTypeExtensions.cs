using SeaBattle.Domain.Models;

namespace SeaBattle.Domain.Extensions
{
    public static class FieldTypeExtensions
    {
        public static bool IsAllowedToFire(this FieldType fieldType)
        {
            return fieldType is FieldType.Boat or FieldType.See;
        }

        public static bool IsAllowedForSafetyZone(this FieldType fieldType)
        {
            return fieldType is FieldType.Boat or FieldType.See;
        }
    }
}
