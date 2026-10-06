For this example , we have chosen to place them in the application layer because they are not part of the core business logic.
They are responsible for fetching and saving data, which is an application concern. 
The domain layer should be focused on business logic and rules, not on how data is stored or retrieved.

One could argue that repositories should be in the domain layer. becuase some domain rules need data access to be implemented. 
However, in this case, we have chosen to keep them in the application layer to maintain a clear separation of concerns and to avoid coupling the domain layer with data access details.

One could also argue that this debate is a waste of time and Domain and Application should not be separated at all.