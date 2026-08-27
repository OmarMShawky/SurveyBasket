namespace SurveyBasket.Services;

public class PollService(ApplicationDbContext context) : IPollService
{
    private readonly ApplicationDbContext _context = context;
    public async Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken)
        => await _context.Polls.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
    public async Task<Poll?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Polls.FindAsync([id], cancellationToken: cancellationToken);
        //return await _context.Polls.FindAsync(id, cancellationToken);
    }

    public async Task<Poll> AddAsync(Poll poll, CancellationToken cancellationToken)
    {
        await _context.Polls.AddAsync(poll, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return poll;
    }

    public async Task<bool> UpdateAsync(int id, Poll poll, CancellationToken cancellationToken)
    {
        var currentPoll = await GetByIdAsync(id, cancellationToken);

        if (currentPoll is null)
            return false;

        currentPoll.Title = poll.Title;
        currentPoll.Summary = poll.Summary;
        currentPoll.StartsAt = poll.StartsAt;
        currentPoll.EndsAt = poll.EndsAt;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var poll = await GetByIdAsync(id, cancellationToken);

        if (poll is null)
            return false;

        _context.Remove(poll);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> TogglePollStatusAsync(int id, CancellationToken cancellationToken)
    {
        var poll = await GetByIdAsync(id, cancellationToken);

        if (poll is null)
            return false;

        poll.IsPublished = !poll.IsPublished;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
