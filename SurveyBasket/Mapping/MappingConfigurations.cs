namespace SurveyBasket.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config = new TypeAdapterConfig();
        config.NewConfig<Poll, PollResponse>(); //.Map(des => des.Note, src => src.Summary);
    }
}
