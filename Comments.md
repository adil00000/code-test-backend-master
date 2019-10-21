Should be one exit point in SubmitApplicationFor
Shouldn't need to throw InvalidOperationException if function reaches the end of the method
Check of null and throw approprate ArgumentNullException with descriptive text
Remove 'is' keyword and replace with case statement
	No need to cast - use it if the case allows it
Refactor the newing of CompanyDataRequest & LoansRequest into its own method (DRY)

Added Tests to confirm correct output for each invoice type
Added test to confirm ArgumentNullException if the service was not correctly set. ie null