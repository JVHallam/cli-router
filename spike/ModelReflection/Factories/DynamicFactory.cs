public static class DynamicFactory
{
    public static dynamic CreateInstance(Type targetType, object[] argsForConstructor)
    {
        dynamic? request = Activator.CreateInstance(targetType, argsForConstructor);

        if(request == null)
        {
            throw new NotImplementedException("The created request was null. Are you sure the args were correct?");
        }

        return request;
    }
}
