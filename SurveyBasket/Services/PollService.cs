namespace SurveyBasket.Services;

public class PollService(ApplicationDbContext context) : IPollService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken)
        => await _context.Polls.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);

    public async Task<Result<PollResponse>> GetAsync(int id, CancellationToken cancellationToken)
    {
        var poll = await _context.Polls.FindAsync([id], cancellationToken);

        return poll is not null
            ? Result.Success(poll.Adapt<PollResponse>())
            : Result.Failure<PollResponse>(PollErrors.NotFound);

    }

    public async Task<Result<PollResponse>> AddAsync(PollRequest request, CancellationToken cancellationToken = default)
    {
        var titleExists = await _context.Polls.AnyAsync(p => p.Title == request.Title, cancellationToken);

        if (titleExists)
            return Result.Failure<PollResponse>(PollErrors.DuplicatedTitle);

        var poll = request.Adapt<Poll>();

        await _context.Polls.AddAsync(poll, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = poll.Adapt<PollResponse>();

        return Result.Success(response);
    }

    public async Task<Result> UpdateAsync(int id, PollRequest pollRequest, CancellationToken cancellationToken = default)
    {
        var currentPoll = await _context.Polls.FindAsync([id], cancellationToken);

        if (currentPoll is null)
            return Result.Failure(PollErrors.NotFound);

        var isTitleDuplicated = await _context.Polls.AnyAsync(p => p.Title == pollRequest.Title && p.Id != id, cancellationToken);

        if(isTitleDuplicated)
            return Result.Failure(PollErrors.DuplicatedTitle);

        currentPoll.Title = pollRequest.Title;
        currentPoll.Summary = pollRequest.Summary;
        currentPoll.StartsAt = pollRequest.StartsAt;
        currentPoll.EndsAt = pollRequest.EndsAt;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var poll = await _context.Polls.FindAsync([id], cancellationToken);

        if (poll is null)
            return Result.Failure<Poll>(PollErrors.NotFound);

        _context.Remove(poll);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> TogglePollStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await _context.Polls.FindAsync([id], cancellationToken);

        if (poll is null)
            return Result.Failure(PollErrors.NotFound);

        poll.IsPublished = !poll.IsPublished;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
