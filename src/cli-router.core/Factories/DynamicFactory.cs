namespace CliRouter.Core.Factories;

public static class DynamicFactory
{
    public static dynamic CreateInstance(Type targetType, object[] argsForConstructor)
    {
        //TODO: Need to handle some types a little better

        if(targetType == typeof(string[]))
        {
            //Then args for constructor has one value, being a string[]
            return argsForConstructor[0];
        }

        dynamic? request = Activator.CreateInstance(targetType, argsForConstructor);

        if(request == null)
        {
            throw new NotImplementedException("The created request was null. Are you sure the args were correct?");
        }

        return request;
    }
}
