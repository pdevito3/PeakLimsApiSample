namespace PeakLims.SharedTestHelpers.Fakes.AccessionComments;

using Bogus;
using Domain.AccessionComments;
using PeakLims.Domain.Accessions;
using PeakLims.SharedTestHelpers.Fakes.Accession;

public class FakeAccessionCommentBuilder 
    : IAccessionSelectionStage
{
    private Accession _accession = null;
    private string _comment;

    private FakeAccessionCommentBuilder() { }
    
    public static IAccessionSelectionStage Initialize() => new FakeAccessionCommentBuilder();

    public FakeAccessionCommentBuilder WithComment(string comment)
    {
        _comment = comment;
        return this;
    }

    public FakeAccessionCommentBuilder WithAccession(Accession accession)
    {
        _accession = accession;
        return this;
    }

    public FakeAccessionCommentBuilder WithMockAccession()
    {
        _accession = FakeAccessionBuilder
            .Initialize()
            .WithMockTestRepository()
            .Build();
        return this;
    }
    
    public AccessionComment Build()
    {
        var faker = new Faker();
        var comment = _comment ?? faker.Lorem.Sentence();
        var accessionComment = AccessionComment.Create(_accession, comment);
        return accessionComment;
    }
}

public interface IAccessionSelectionStage
{
    public FakeAccessionCommentBuilder WithAccession(Accession accession);
    public FakeAccessionCommentBuilder WithMockAccession();
}