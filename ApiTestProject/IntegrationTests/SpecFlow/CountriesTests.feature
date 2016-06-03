Feature: CountriesTests
	In order for addresses to have associated countries
	As an EHIM user
	I want to be able to manage the country data in our system

Scenario: Create a country
	Given the following countries
	| Alpha2Code | Alpha3Code | EffectiveEndDate | EffectiveStartDate | FullName                     | Iso3166Code | PhoneNumberRegex                                      | PostalCodeRegex   | ShortName                |
	| US         | USA        |                  | 7/4/1776           | the United States of America | 840         | ^1?[. -]?\(?\d{3}\)?[. -]? *\d{3}[. -]? *[. -]?\d{4}$ | ^\d{5}[ -]?\d{4}$ | United States Of America |
	When post is invoked on the countries api
	Then the country should be returned with an id

Scenario: Get a country
	Given the following countries exist in the data store
	| Alpha2Code | Alpha3Code | EffectiveEndDate | EffectiveStartDate | FullName                     | Iso3166Code | PhoneNumberRegex                                      | PostalCodeRegex   | ShortName                |
	| US         | USA        |                  | 7/4/1776           | the United States of America | 840         | ^1?[. -]?\(?\d{3}\)?[. -]? *\d{3}[. -]? *[. -]?\d{4}$ | ^\d{5}[ -]?\d{4}$ | United States Of America |
	When get is invoked on the countries api with the country's id
	Then the country should be returned

Scenario: Update a country
	Given the following countries exist in the data store
	| Alpha2Code | Alpha3Code | EffectiveEndDate | EffectiveStartDate | FullName                     | Iso3166Code | PhoneNumberRegex                                      | PostalCodeRegex   | ShortName                |
	| SU         | ASU        | 12/31/3000       | 4/7/1767           | the United Staets of America | 804         | ^1?[. -]?\?[. -]? *\d{3}[. -]? *[. -]?\d{4}$          | ^\d{5}[ -]?$      | United Staets Of America |
	When update is invoked on the countries api with the following values
	| Alpha2Code | Alpha3Code | EffectiveEndDate | EffectiveStartDate | FullName                     | Iso3166Code | PhoneNumberRegex                                      | PostalCodeRegex   | ShortName                |
	| US         | USA        |                  | 7/4/1776           | the United States of America | 840         | ^1?[. -]?\(?\d{3}\)?[. -]? *\d{3}[. -]? *[. -]?\d{4}$ | ^\d{5}[ -]?\d{4}$ | United States Of America |
	Then the country should be updated

Scenario: Soft delete a country
	Given the following countries exist in the data store
	| Alpha2Code | Alpha3Code | EffectiveEndDate | EffectiveStartDate | FullName                     | Iso3166Code | PhoneNumberRegex                                      | PostalCodeRegex   | ShortName                |
	| US         | USA        |                  | 7/4/1776           | the United States of America | 840         | ^1?[. -]?\(?\d{3}\)?[. -]? *\d{3}[. -]? *[. -]?\d{4}$ | ^\d{5}[ -]?\d{4}$ | United States Of America |
	When delete is invoked on the countries api with the country's id
	Then the country should exist in the data store
	And the country's effective end date should be updated

Scenario: Delete a country
	Given the following countries exist in the data store
	| Alpha2Code | Alpha3Code | EffectiveEndDate | EffectiveStartDate | FullName                     | Iso3166Code | PhoneNumberRegex                                      | PostalCodeRegex   | ShortName                |
	| US         | USA        |                  | 7/4/1776           | the United States of America | 840         | ^1?[. -]?\(?\d{3}\)?[. -]? *\d{3}[. -]? *[. -]?\d{4}$ | ^\d{5}[ -]?\d{4}$ | United States Of America |
	When force delete is invoked on the countries api with the country's id
	Then the country should not exist in the data store