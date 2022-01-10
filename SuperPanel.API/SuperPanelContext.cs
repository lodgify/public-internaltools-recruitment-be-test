using Microsoft.EntityFrameworkCore;
using SuperPanel.API.Model;

namespace SuperPanel.API
{
    public class SuperPanelContext : DbContext
    {
        public SuperPanelContext(DbContextOptions<SuperPanelContext> options)
            : base(options)
        { }

        public DbSet<DeletionRequest> DeletionRequests { get; set; }

    }
}