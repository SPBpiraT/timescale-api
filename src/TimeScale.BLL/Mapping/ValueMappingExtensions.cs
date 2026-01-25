using TimeScale.BLL.Models.Value;
using TimeScale.DAL.Entities;

namespace TimeScale.BLL.Mapping
{
    internal static class ValueMappingExtensions
    {
        internal static ValueEntity MapToEntity(this ValueDto valueDto)
        {
            return new()
            {
                FileName = valueDto.FileName,
                Date = valueDto.Date,
                ExecutionTime = valueDto.ExecutionTime,
                Value = valueDto.Value
            };
        }

        internal static ValueDto MapToDto(this ValueEntity valueEntity)
        {
            return new()
            {
                FileName = valueEntity.FileName,
                Date = valueEntity.Date,
                ExecutionTime = valueEntity.ExecutionTime,
                Value = valueEntity.Value
            };
        }    
    }
}
