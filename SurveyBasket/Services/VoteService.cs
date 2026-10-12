namespace SurveyBasket.Services;

public class VoteService(ApplicationDbContext context) : IVoteService
{
    private readonly ApplicationDbContext _context = context;
    public async Task<Result> AddAsync(int pollId, string userId, VoteRequest request, CancellationToken cancellationToken = default)
    {
        var pollIsRunning = await _context.Polls
            .AnyAsync(p => p.Id == pollId &&
                               p.IsPublished &&
                               p.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) &&
                               p.EndsAt > DateOnly.FromDateTime(DateTime.UtcNow),
                               cancellationToken);
        if (!pollIsRunning)
            return Result.Failure(PollErrors.PollNotFound);

        var availableQuestions = await _context.Questions
            .Where(q => q.PollId == pollId && q.IsActive)
            .Select(q => q.Id)
            .ToListAsync(cancellationToken);

        if (!request.Answers.Select(a => a.QuestionId).SequenceEqual(availableQuestions))
            return Result.Failure(VoteErrors.InvalidQuestions);

        // Every answer belongs to its question and is active

        var validPairs = _context.Answers
                .Where(a => a.Question.PollId == pollId && a.Question.IsActive && a.IsActive)
                .Select(a => new { a.Id, a.QuestionId })
                .ToList()
            .Select(a => (a.Id, a.QuestionId))
            .ToHashSet();
    

        if (!request.Answers.All(a => validPairs.Contains((a.AnswerId, a.QuestionId))))
            return Result.Failure(VoteErrors.InvalidAnswers);

        var vote = new Vote
        {
            PollId = pollId,
            UserId = userId,
            VoteAnswers = request.Answers.Adapt<List<VoteAnswer>>()
        };

        await _context.AddAsync(vote, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

}
