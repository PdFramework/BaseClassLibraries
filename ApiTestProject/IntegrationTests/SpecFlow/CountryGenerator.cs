namespace IntegrationTests.SpecFlow
{
    using PeinearyDevelopment.Framework.BaseClassLibraries.Testing.DataGenerator;
    using PeinearyDevelopment.Framework.BaseClassLibraries.Testing.SpecFlow;

    using Contracts;

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class CountryGenerator : IItemsGenerator<Country, int>
    {
        public Country GenerateValidItem()
        {
            var effectiveStartDate = RandomDataGenerator.GenerateRandomDateTimeOffset();

            return new Country
            {
                Alpha2Code = Alpha2Code,
                Alpha3Code = Alpha3Code,
                CreatedByUserId = NonServiceGeneratedCreatedById,
                CreatedOn = NonServiceGeneratedCountryCreatedOn,
                EffectiveEndDate = RandomDataGenerator.GenerateRandomDateTimeOffset(effectiveStartDate),
                EffectiveStartDate = effectiveStartDate,
                FullName = Name,
                Id = NonServiceGeneratedId,
                Iso3166Code = Iso3166Code,
                LastUpdatedByUserId = NonServiceGeneratedLastUpdatedById,
                LastUpdatedOn = NonServiceGeneratedCountryLastUpdatedOn,
                PhoneNumberRegex = PhoneNumberRegex,
                PostalCodeRegex = PostalCodeRegex,
                ShortName = ShortName
            };
        }

        public Country GenerateValidUpdatedItem(int id)
        {
            var country = GenerateValidItem();
            country.Id = id;
            return country;
        }

        private static DateTimeOffset NonServiceGeneratedCountryCreatedOn => RandomDataGenerator.GenerateRandomDateTimeOffset();
        private static DateTimeOffset NonServiceGeneratedCountryLastUpdatedOn => RandomDataGenerator.GenerateRandomDateTimeOffset();

        private static int NonServiceGeneratedCreatedById => RandomDataGenerator.GenerateRandomInt32();
        private static int NonServiceGeneratedCreatedByIdForUpdate => RandomDataGenerator.GenerateRandomInt32();
        private static int NonServiceGeneratedId => RandomDataGenerator.GenerateRandomInt32();
        private static int NonServiceGeneratedLastUpdatedById => RandomDataGenerator.GenerateRandomInt32();

        private static string Alpha2Code => RandomDataGenerator.GenerateRandomString(2);
        private static string Alpha3Code => RandomDataGenerator.GenerateRandomString(3);
        private static int Iso3166Code => GenerateRandomIso3166Code();
        private static string Name => RandomDataGenerator.GenerateRandomString(50);
        private static string PhoneNumberRegex => GenerateRandomPhoneNumberRegex();
        private static string PostalCodeRegex => GenerateRandomPostalCodeRegex();
        private static string ShortName => RandomDataGenerator.GenerateRandomString(10);

        private static int GenerateRandomIso3166Code()
        {
            return RandomDataGenerator.GenerateRandomInt32(999);//.ToString("D3");
        }

        private static string GenerateRandomPhoneNumberRegex()
        {
            return RandomDataGenerator.GenerateRandomItemFromEnumerable<ReadOnlyDictionary<string, string>, KeyValuePair<string, string>>(PhoneNumberRegexes).Value;
        }

        private static string GenerateRandomPostalCodeRegex()
        {
            return RandomDataGenerator.GenerateRandomItemFromEnumerable<ReadOnlyDictionary<string, string>, KeyValuePair<string, string>>(PostalRegexes).Value;
        }

        /* Regexes from here: http://regexlib.com/Search.aspx?k=postal+code
         * POSTAL CODES *
         * India: ^[1-9]{3}\s{0,1}[0-9]{3}$
         * Brazil: ^\d{2}(\x2e)(\d{3})(-\d{3})?$
         * Canada: ^[aAbBcCeEgGhHjJkKlLmMnNpPrRsStTvVxXyY]\d[aAbBcCeEgGhHjJkKlLmMnNpPrRsStTvVwWxXyYzZ]( )?\d[aAbBcCeEgGhHjJkKlLmMnNpPrRsStTvVwWxXyYzZ]\d$
         * Netherlands: ^[1-9][0-9]{3}\s?[a-zA-Z]{2}$
         * Denmark: ^[D-d][K-k]( |-)[1-9]{1}[0-9]{3}$
         * United States: ^\d{5}[ -]?\d{4}$
         * United Kingdom: ^[A-Za-z]{1,2}[\d]{1,2}([A-Za-z])?\s?[\d][A-Za-z]{2}$
         * Russia: ^[0-9]{6}
         * Germany: \b((?:0[1-46-9]\d{3})|(?:[1-357-9]\d{4})|(?:[4][0-24-9]\d{3})|(?:[6][013-9]\d{3}))\b
         * Switzerland: ^[1-9]{1}[0-9]{3}$
         * Australia: (0[289][0-9]{2})|([1345689][0-9]{3})|(2[0-8][0-9]{2})|(290[0-9])|(291[0-4])|(7[0-4][0-9]{2})|(7[8-9][0-9]{2})
         * Latvia: ^(LV-)[0-9]{4}$
         * France: ((^[0-9]*).?((BIS)|(TER)|(QUATER))?)?((\W+)|(^))(([a-z]+.)*)([0-9]{5})?.(([a-z\'']+.)*)$
         * Slovakia: ^(([0-9]{5})|([0-9]{3}[ ]{0,1}[0-9]{2}))$
         * Portugal: [0-9]{4}-[0-9]{3}
         */
        private static readonly ReadOnlyDictionary<string, string> PostalRegexes = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>
        {
             { "Australia", @"(0[289][0-9]{2})|([1345689][0-9]{3})|(2[0-8][0-9]{2})|(290[0-9])|(291[0-4])|(7[0-4][0-9]{2})|(7[8-9][0-9]{2})" },
             { "Brazil", @"^\d{2}(\x2e)(\d{3})(-\d{3})?$" },
             { "Canada", @"^[aAbBcCeEgGhHjJkKlLmMnNpPrRsStTvVxXyY]\d[aAbBcCeEgGhHjJkKlLmMnNpPrRsStTvVwWxXyYzZ]( )?\d[aAbBcCeEgGhHjJkKlLmMnNpPrRsStTvVwWxXyYzZ]\d$" },
             { "Denmark", @"^[D-d][K-k]( |-)[1-9]{1}[0-9]{3}$" },
             { "France", @"((^[0-9]*).?((BIS)|(TER)|(QUATER))?)?((\W+)|(^))(([a-z]+.)*)([0-9]{5})?.(([a-z\'']+.)*)$" },
             { "Germany", @"\b((?:0[1-46-9]\d{3})|(?:[1-357-9]\d{4})|(?:[4][0-24-9]\d{3})|(?:[6][013-9]\d{3}))\b" },
             { "India", @"^[1-9]{3}\s{0,1}[0-9]{3}$" },
             { "Latvia", @"^(LV-)[0-9]{4}$" },
             { "Netherlands", @"^[1-9][0-9]{3}\s?[a-zA-Z]{2}$" },
             { "Portugal", @"[0-9]{4}-[0-9]{3}" },
             { "Russia", @"^[0-9]{6}" },
             { "Slovakia", @"^(([0-9]{5})|([0-9]{3}[ ]{0,1}[0-9]{2}))$" },
             { "Switzerland", @"^[1-9]{1}[0-9]{3}$" },
             { "United Kingdom", @"^[A-Za-z]{1,2}[\d]{1,2}([A-Za-z])?\s?[\d][A-Za-z]{2}$" },
             { "United States", @"^\d{5}[ -]?\d{4}$" }
        });

        /* Regexes from here: http://regexlib.com/Search.aspx?k=phone+number
         * PHONE NUMBERS *
         * Brazil: \(([0-9]{2}|0{1}((x|[0-9]){2}[0-9]{2}))\)\s*[0-9]{3,4}[- ]*[0-9]{4}
         * Australia: (^1300\d{6}$)|(^1800|1900|1902\d{6}$)|(^0[2|3|7|8]{1}[0-9]{8}$)|(^13\d{4}$)|(^04\d{2,3}\d{6}$)
         * Dutch: (^\+[0-9]{2}|^\+[0-9]{2}\(0\)|^\(\+[0-9]{2}\)\(0\)|^00[0-9]{2}|^0)([0-9]{9}$|[0-9\-\s]{10}$)
         * Israel: ^0[23489]{1}(\-)?[^0\D]{1}\d{6}$
         * Germany: ^\(\d{1,2}(\s\d{1,2}){1,2}\)\s(\d{1,2}(\s\d{1,2}){1,2})((-(\d{1,4})){0,1})$
         * United Kingdom: (\s*\(?0\d{4}\)?\s*\d{6}\s*)|(\s*\(?0\d{3}\)?\s*\d{3}\s*\d{4}\s*)
         * Sweden: ^(([+]\d{2}[ ][1-9]\d{0,2}[ ])|([0]\d{1,3}[-]))((\d{2}([ ]\d{2}){2})|(\d{3}([ ]\d{3})*([ ]\d{2})+))$
         * Italy: ^([0-9]*\-?\ ?\/?[0-9]*)$
         * United Arab Emirates: ^(\+97[\s]{0,1}[\-]{0,1}[\s]{0,1}1|0)50[\s]{0,1}[\-]{0,1}[\s]{0,1}[1-9]{1}[0-9]{6}$
         * India: ^0{0,1}[1-9]{1}[0-9]{2}[\s]{0,1}[\-]{0,1}[\s]{0,1}[1-9]{1}[0-9]{6}$
         * New Zealand: (^\([0]\d{2}\))(\d{6,7}$)
         * Russia: ((8|\+7)-?)?\(?\d{3,5}\)?-?\d{1}-?\d{1}-?\d{1}-?\d{1}-?\d{1}((-?\d{1})?-?\d{1})?
         * South Africa: (^0[87][23467]((\d{7})|( |-)((\d{3}))( |-)(\d{4})|( |-)(\d{7})))
         * Slovakia: ^(([0-9]{3})[ \-\/]?([0-9]{3})[ \-\/]?([0-9]{3}))|([0-9]{9})|([\+]?([0-9]{3})[ \-\/]?([0-9]{2})[ \-\/]?([0-9]{3})[ \-\/]?([0-9]{3}))$
         * Danish: ^((\(?\+45\)?)?)(\s?\d{2}\s?\d{2}\s?\d{2}\s?\d{2})$
         * Croatia: ^((\d{2,4})/)?((\d{6,8})|(\d{2})-(\d{2})-(\d{2,4})|(\d{3,4})-(\d{3,4}))$
         * Belgium: ^0[1-9]\d{7,8}$
         * Ukraine: ^((8|\+38)-?)?(\(?044\)?)?-?\d{3}-?\d{2}-?\d{2}$
         * Chile: ^((\(\d{3}\) ?)|(\d{3}-)|(\(\d{2}\) ?)|(\d{2}-)|(\(\d{1}\) ?)|(\d{1}-))?\d{3}-(\d{3}|\d{4})
         * Poland: ^(\+48\s+)?\d{3}(\s*|\-)\d{3}(\s*|\-)\d{3}$
         * Czechoslovakia: [^0-9]((\(?(\+420|00420)\)?( |-)?)?([0-9]{3} ?(([0-9]{3} ?[0-9]{3})|([0-9]{2} ?[0-9]{2} ?[0-9]{2})))|([0-9]{3}-(([0-9]{3}-[0-9]{3})|([0-9]{2}-[0-9]{2}-[0-9]{2}))))[^0-9|/]
         * Ireland: \+353\(0\)\s\d\s\d{3}\s\d{4}
         * Chennai: ^(0)44[\s]{0,1}[\-]{0,1}[\s]{0,1}2[\s]{0,1}[1-9]{1}[0-9]{6}$
         */
        private static readonly ReadOnlyDictionary<string, string> PhoneNumberRegexes = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>
        {
             { "Brazil", @"\(([0-9]{2}|0{1}((x|[0-9]){2}[0-9]{2}))\)\s*[0-9]{3,4}[- ]*[0-9]{4}" },
             { "Australia", @"(^1300\d{6}$)|(^1800|1900|1902\d{6}$)|(^0[2|3|7|8]{1}[0-9]{8}$)|(^13\d{4}$)|(^04\d{2,3}\d{6}$)" },
             { "Dutch", @"(^\+[0-9]{2}|^\+[0-9]{2}\(0\)|^\(\+[0-9]{2}\)\(0\)|^00[0-9]{2}|^0)([0-9]{9}$|[0-9\-\s]{10}$)" },
             { "Israel", @"^0[23489]{1}(\-)?[^0\D]{1}\d{6}$" },
             { "Germany", @"^\(\d{1,2}(\s\d{1,2}){1,2}\)\s(\d{1,2}(\s\d{1,2}){1,2})((-(\d{1,4})){0,1})$" },
             { "United Kingdom", @"(\s*\(?0\d{4}\)?\s*\d{6}\s*)|(\s*\(?0\d{3}\)?\s*\d{3}\s*\d{4}\s*)" },
             { "Sweden", @"^(([+]\d{2}[ ][1-9]\d{0,2}[ ])|([0]\d{1,3}[-]))((\d{2}([ ]\d{2}){2})|(\d{3}([ ]\d{3})*([ ]\d{2})+))$" },
             { "Italy", @"^([0-9]*\-?\ ?\/?[0-9]*)$" },
             { "United Arab Emirates", @"^(\+97[\s]{0,1}[\-]{0,1}[\s]{0,1}1|0)50[\s]{0,1}[\-]{0,1}[\s]{0,1}[1-9]{1}[0-9]{6}$" },
             { "India", @"^0{0,1}[1-9]{1}[0-9]{2}[\s]{0,1}[\-]{0,1}[\s]{0,1}[1-9]{1}[0-9]{6}$" },
             { "New Zealand", @"(^\([0]\d{2}\))(\d{6,7}$)" },
             { "Russia", @"((8|\+7)-?)?\(?\d{3,5}\)?-?\d{1}-?\d{1}-?\d{1}-?\d{1}-?\d{1}((-?\d{1})?-?\d{1})?" },
             { "South Africa", @"(^0[87][23467]((\d{7})|( |-)((\d{3}))( |-)(\d{4})|( |-)(\d{7})))" },
             { "Slovakia", @"^(([0-9]{3})[ \-\/]?([0-9]{3})[ \-\/]?([0-9]{3}))|([0-9]{9})|([\+]?([0-9]{3})[ \-\/]?([0-9]{2})[ \-\/]?([0-9]{3})[ \-\/]?([0-9]{3}))$" },
             { "Danish", @"^((\(?\+45\)?)?)(\s?\d{2}\s?\d{2}\s?\d{2}\s?\d{2})$" },
             { "Croatia", @"^((\d{2,4})/)?((\d{6,8})|(\d{2})-(\d{2})-(\d{2,4})|(\d{3,4})-(\d{3,4}))$" },
             { "Belgium", @"^0[1-9]\d{7,8}$" },
             { "Ukraine", @"^((8|\+38)-?)?(\(?044\)?)?-?\d{3}-?\d{2}-?\d{2}$" },
             { "Chile", @"^((\(\d{3}\) ?)|(\d{3}-)|(\(\d{2}\) ?)|(\d{2}-)|(\(\d{1}\) ?)|(\d{1}-))?\d{3}-(\d{3}|\d{4})" },
             { "Poland", @"^(\+48\s+)?\d{3}(\s*|\-)\d{3}(\s*|\-)\d{3}$" },
             { "Czechoslovakia", @"[^0-9]((\(?(\+420|00420)\)?( |-)?)?([0-9]{3} ?(([0-9]{3} ?[0-9]{3})|([0-9]{2} ?[0-9]{2} ?[0-9]{2})))|([0-9]{3}-(([0-9]{3}-[0-9]{3})|([0-9]{2}-[0-9]{2}-[0-9]{2}))))[^0-9|/]" },
             { "Ireland", @"\+353\(0\)\s\d\s\d{3}\s\d{4}" },
             { "Chennai", @"^(0)44[\s]{0,1}[\-]{0,1}[\s]{0,1}2[\s]{0,1}[1-9]{1}[0-9]{6}$" }
        });

        /*TODO: add fare from here: https://github.com/moodmosaic/Fare
         * EXAMPLE USAGE
         * var a = @"^1?[. -]?\(?\d{3}\)?[. -]? *\d{3}[. -]? *[. -]?\d{4}$";
         * var b = new Fare.Xeger(a);
         * var c = b.Generate();
        */
    }
}
