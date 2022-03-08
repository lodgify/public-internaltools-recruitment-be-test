namespace SuperPanel.App.Models.DTO.Base
{
    public class SingleQueryResult<TEntity> : ResultBase
    {
        public TEntity Entity { get; set; }
    }
}
