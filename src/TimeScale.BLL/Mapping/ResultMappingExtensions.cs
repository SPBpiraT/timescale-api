using TimeScale.BLL.Models.Result;
using TimeScale.DAL.Entities;

namespace TimeScale.BLL.Mapping
{
    internal static class ResultMappingExtensions
    {
        internal static ResultEntity MapToEntity(this ResultDto resultDto)
        {
            return new()
            {
                FileName = resultDto.FileName,
                DeltaTime = resultDto.DeltaTime,
                FirstOperationDate = resultDto.FirstOperationDate,
                AvgExecutionTime = resultDto.AvgExecutionTime,
                AvgValue = resultDto.AvgValue,
                MedianValue = resultDto.MedianValue,
                MaxValue = resultDto.MaxValue,
                MinValue = resultDto.MinValue
            };
        }

        internal static ResultDto MapToDto(this ResultEntity resultEntity)
        {
            return new()
            {
                FileName = resultEntity.FileName,
                DeltaTime = resultEntity.DeltaTime,
                FirstOperationDate = resultEntity.FirstOperationDate,
                AvgExecutionTime = resultEntity.AvgExecutionTime,
                AvgValue = resultEntity.AvgValue,
                MedianValue = resultEntity.MedianValue,
                MaxValue = resultEntity.MaxValue,
                MinValue = resultEntity.MinValue
            };
        }
    }
}
