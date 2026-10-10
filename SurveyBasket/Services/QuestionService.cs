namespace SurveyBasket.Services;

public class QuestionService(ApplicationDbContext context) : IQuestionService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId, CancellationToken cancellationToken = default)
    {
        var pollExists = await _context.Polls.AnyAsync(p => p.Id == pollId, cancellationToken);

        if (!pollExists)
            return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.NotFound);

        var questions = await _context.Questions
            .Where(q => q.PollId == pollId)
            .Include(q => q.Answers)
            .ProjectToType<QuestionResponse>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        //.Select(q => new QuestionResponse(
        //    q.Id,
        //    q.Content,
        //    q.Answers.Select(answer => new AnswerResponse(answer.Id, answer.Content))

        //))


        return Result.Success<IEnumerable<QuestionResponse>>(questions);
    }
    public async Task<Result<QuestionResponse>> GetAsync(int pollId, int id, CancellationToken cancellationToken = default)
    {
        var question = await _context.Questions
            .Where(q => q.Id == id && q.PollId == pollId)
            .Include(q => q.Answers)
            .ProjectToType<QuestionResponse>()
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);

        if (question is null)
            return Result.Failure<QuestionResponse>(QuestionErrors.NotFound);

        return Result.Success(question);
    }

    public async Task<Result<QuestionResponse>> Addsync(int pollId, QuestionRequest request, CancellationToken cancellationToken = default)
    {
        var pollExists = await _context.Polls.AnyAsync(p => p.Id == pollId, cancellationToken);

        if (!pollExists)
            return Result.Failure<QuestionResponse>(PollErrors.NotFound);

        var contentExists = await _context.Questions
            .AnyAsync(q => q.Content == request.Content && q.PollId == pollId, cancellationToken);

        if (contentExists)
            return Result.Failure<QuestionResponse>(QuestionErrors.DuplicatedContent);

        var question = request.Adapt<Question>();

        question.PollId = pollId;

        await _context.AddAsync(question, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(question.Adapt<QuestionResponse>());
    }

    public async Task<Result> UpdateAsync(int pollId, int id, QuestionRequest request, CancellationToken cancellationToken = default)
    {
        var ContentExists = await _context.Questions
            .AnyAsync(q => q.PollId == pollId && q.Id != id && q.Content == request.Content, cancellationToken);

        if (ContentExists)
            return Result.Failure(QuestionErrors.DuplicatedContent);


        var question = await _context.Questions
            .Include(q => q.Answers)
            .SingleOrDefaultAsync(q => q.Id == id && q.PollId == pollId, cancellationToken);

        if (question is null)
            return Result.Failure(QuestionErrors.NotFound);

        question.Content = request.Content;

        var currentAnswers = question.Answers.Select(a => a.Content).ToList();

        var newAnswers = request.Answers.Except(currentAnswers).ToList();

        newAnswers.ForEach(answer =>
        {
            question.Answers.Add(new Answer { Content = answer });
        });

        question.Answers.ToList().ForEach(answer =>
        {
            answer.IsActive = request.Answers.Contains(answer.Content);
        });

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> ToggleStatusAsync(int pollId, int id, CancellationToken cancellationToken = default)
    {
        var question = await _context.Questions
            .Where(q => q.Id == id && q.PollId == pollId)
            .SingleOrDefaultAsync(cancellationToken);

        //var question = await _context.Questions
        //    .SingleOrDefaultAsync(q => q.Id == id && q.PollId == pollId, cancellationToken: cancellationToken);

        if (question is null)
            return Result.Failure<QuestionResponse>(QuestionErrors.NotFound);

        question.IsActive = !question.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(question.Adapt<QuestionResponse>());

    }
}