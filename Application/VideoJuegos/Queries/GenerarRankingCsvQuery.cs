using MediatR;

public class GenerarRankingCsvQuery : IRequest<string>
{
    public int Top { get; set; }

    public GenerarRankingCsvQuery(int top)
    {
        Top = top;
    }
}
