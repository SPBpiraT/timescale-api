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
            return new(resultEntity.FileName,
                resultEntity.DeltaTime,
                resultEntity.FirstOperationDate,
                resultEntity.AvgExecutionTime,
                resultEntity.AvgValue,
                resultEntity.MedianValue,
                resultEntity.MaxValue,
                resultEntity.MinValue);
        }
    }
}
