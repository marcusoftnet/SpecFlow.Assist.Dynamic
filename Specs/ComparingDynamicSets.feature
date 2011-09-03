
Feature: Comparing dynamic sets against tables
	In order to easier and slicker do assertions
	As a SpecFlow developer
	I want to be able to compare a list of dynamic items against a table
	
Scenario: Comparing against an identical table should match
	Given I create a set of dynamic instances from this table
		| Name   | Age | Birth date | Length in meters |
		| Marcus | 39  | 1972-10-09 | 1.96             |
		| Albert | 3   | 2008-01.24 | 1.03             |
		| Gustav | 1   | 2010-03-19 | 0.84             |
		| Arvid  | 1   | 2010-03-19 | 0.85             |
	When I compare the set to this table
		| Name   | Age | Birth date | Length in meters |
		| Marcus | 39  | 1972-10-09 | 1.96             |
		| Albert | 3   | 2008-01.24 | 1.03             |
		| Gustav | 1   | 2010-03-19 | 0.84             |
		| Arvid  | 1   | 2010-03-19 | 0.85             |
	Then no set comparison exception should have been thrown

Scenario: Not matching when 1 column name differ
	Given I create a set of dynamic instances from this table
		| Name   | 
		| Marcus | 
		| Albert | 
		| Gustav | 
		| Arvid  | 
	When I compare the set to this table
		| N      |
		| Marcus |
		| Albert |
		| Gustav |
		| Arvid  |
	Then an set comparision exception should be thrown with 2 differences
		And one set difference should be on the 'Name' field of the instance
		And one set difference should be on the 'N' column of the table

Scenario: Not matching when 2 header differ
	Given I create a set of dynamic instances from this table
		| Name   | Age |
		| Marcus | 39  |
		| Albert | 3   |
		| Gustav | 1   |
		| Arvid  | 1   |
	When I compare the set to this table
		| Namn   | Ålder |
		| Marcus | 39    |
		| Albert | 3     |
		| Gustav | 1     |
		| Arvid  | 1     |
	Then an set comparision exception should be thrown with 4 differences
		And one set difference should be on the 'Name' field of the instance
		And one set difference should be on the 'Age' field of the instance
		And one set difference should be on the 'Namn' column of the table
		And one set difference should be on the 'Ålder' column of the table

Scenario: Not matching when the number of rows are more in the table
	Given I create a set of dynamic instances from this table
		| Name   | Age |
		| Marcus | 39  |
		| Albert | 3   |
	When I compare the set to this table
		| Name   | Age |
		| Marcus | 39  |
		| Albert | 3   |
		| Arvid  | 1   |
	Then an set comparison exception should be thrown
		And the error message for different rows should expect 3 rows for table and 2 rows for instance
		