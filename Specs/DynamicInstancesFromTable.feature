Feature: Create dynamic objects from SpecFlow table
	In order to write only code that matters
	As a SpecFlow developer
	I want SpecFlow to create dynamic objects from a table row

Scenario: Create dynamic instance from table with one row
	When I create a dynamic instance from this table
		| Name   | Age | Birth date | Length in meters |
		| Marcus | 39  | 1972-10-09 | 1.96             |
	Then the Name property should equal 'Marcus'
		And the Age property should equal 39
		And the BirthDate property should equal 1972-10-09
		And the LengthInMeters property should equal 1.96

Scenario: Create dynamic instance from table with one row and 2 columns
	When I create a dynamic instance from this table
		| Name   | Age | 
		| Marcus | 39  | 
	Then the Name property should equal 'Marcus'
		And the Age property should equal 39

Scenario: Create dynamic instance from table with Field and Values
	When I create a dynamic instance from this table
		| Field            | Value      |
		| Name             | Marcus     |
		| Age              | 39         |
		| Birth date       | 1972-10-09 |
		| Length in meters | 1.96       |
	Then the Name property should equal 'Marcus'
		And the Age property should equal 39
		And the BirthDate property should equal 1972-10-09
		And the LengthInMeters property should equal 1.96

#Scenario: Matching a dynamic instance against a table
#	Given I create a dynamic instance from this table
#		| Name   | Age | Birth date | Length in meters |
#		| Marcus | 39  | 1972-10-09 | 1.96             |
#	When I compare the dynamic instance against this object
#		| Name   | Age | Birth date | Length in meters |
#		| Marcus | 39  | 1972-10-09 | 1.96             |
#	Then no exception should have been thrown
#
#Scenario: Not matching when headers differ
#	Given I create a dynamic instance from this table
#		| Name   | 
#		| Marcus | 
#	When I compare it to this table	 
#	   | N      |
#	   | Marcus |
#	Then an exception that tells me that the property name differs should have been thrown