Feature: Conversions of values
	In order to easier compare values of the most common types
	As a user of SpecFlow Dynamic
	I want SpecFlow Dynamic to translate strings into the closest ressembling real type


Scenario: Strings should be translated to string
	When I create a dynamic instance from this table
		| Name   |
		| Marcus |
	Then the Name property should equal 'Marcus'
