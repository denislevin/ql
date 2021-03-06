using Microsoft.CodeAnalysis.CSharp.Syntax;
using Semmle.Extraction.Entities;
using System.IO;

namespace Semmle.Extraction.CSharp.Entities
{
    internal abstract class PreprocessorDirective<TDirective> : FreshEntity where TDirective : DirectiveTriviaSyntax
    {
        protected readonly TDirective trivia;

        protected PreprocessorDirective(Context cx, TDirective trivia, bool populateFromBase = true)
            : base(cx)
        {
            this.trivia = trivia;
            if (populateFromBase)
            {
                TryPopulate();
            }
        }

        protected sealed override void Populate(TextWriter trapFile)
        {
            PopulatePreprocessor(trapFile);

            trapFile.preprocessor_directive_active(this, trivia.IsActive);
            trapFile.preprocessor_directive_location(this, cx.CreateLocation(ReportingLocation));

            if (!cx.Extractor.Standalone)
            {
                var compilation = Compilation.Create(cx);
                trapFile.preprocessor_directive_compilation(this, compilation);
            }
        }

        protected abstract void PopulatePreprocessor(TextWriter trapFile);

        public sealed override Microsoft.CodeAnalysis.Location ReportingLocation => trivia.GetLocation();

        public override TrapStackBehaviour TrapStackBehaviour => TrapStackBehaviour.OptionalLabel;
    }
}
