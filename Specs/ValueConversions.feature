Feature: Conversions of values
	In order to easier compare values of the most common types
	As a user of SpecFlow Dynamic
	I want SpecFlow Dynamic to translate strings into the closest ressembling real type


Scenario: Strings should be translated to string
	When I create a dynamic instance from this table
		| Name   |
		| Marcus |
	Then the Name property should equal 'Marcus'

Scenario: Integers should be translated from strings
	When I create a dynamic instance from this table
		| Age |
		| 39  |
	Then the Age property should equal 39

Scenario: Doubles should be translated from strings
	When I create a dynamic instance from this table
		| Length in meters |
		| 1.96             | 
	Then the LengthInMeters property should equal '1.96'

Scenario: Dates should be translated from strings
	When I create a dynamic instance from this table
		| Birth date |
		| 1972-10-09 | 
	Then the BirthDate property should equal 1972-10-09

Scenario: Bools should be translated from strings
	When I create a dynamic instance from this table
		| Is developer |
		| false        | 
	Then the IsDeveloper property should equal 'false'

Scenario: A strange double should not be translated into a date
	When I create a dynamic instance from this table
		| Length in meters |
		| 4.567            |  
	Then the LengthInMeters property should equal '4.567'