namespace PeakLims.SharedTestHelpers.Fakes.Accession;

using AutoBogus;
using PeakLims.Domain.Accessions;
using PeakLims.Domain.Accessions.Dtos;

public class FakeAccession
{
    public static Accession Generate(AccessionForCreationDto accessionForCreationDto)
    {
        return Accession.Create(accessionForCreationDto);
    }

    public static Accession Generate()
    {
        return Accession.Create(new FakeAccessionForCreationDto().Generate());
    }
}