using MediatR;

public record GetPatientsQuery(int Page = 1, int PageSize = 1000) : IRequest<GetPatientsResult>; //set to 1000 from 10
