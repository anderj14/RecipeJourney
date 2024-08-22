
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class InstructionConfig : IEntityTypeConfiguration<Instruction>
    {
        public void Configure(EntityTypeBuilder<Instruction> builder)
        {
            builder.Property(x => x.StepNumber).IsRequired();
            builder.Property(x => x.Description).IsRequired().HasMaxLength(500);
        }
    }
}