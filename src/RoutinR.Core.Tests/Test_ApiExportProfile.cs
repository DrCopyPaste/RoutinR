using RoutinR.Constants;

namespace RoutinR.Core.Tests
{
    public class Test_ApiExportProfile
    {
        [Fact]
        public void Cannot_create_export_profile_without_name()
        {
            var expectedException = false;
            try
            {
                var profile = new ApiExportProfile(name: string.Empty, "https://github.com");
            }
            catch (ArgumentNullException)
            {
                expectedException = true;
            }
            Assert.True(expectedException, "creating an api export profile without name did not raise the expected exception");
        }

        [Fact]
        public void Cannot_create_export_profile_without_a_post_url()
        {
            var expectedException = false;
            try
            {
                var profile = new ApiExportProfile(name: "Name", string.Empty);
            }
            catch (ArgumentNullException)
            {
                expectedException = true;
            }
            Assert.True(expectedException, "creating an api export profile without name did not raise the expected exception");
        }

        [Fact]
        public void Post_url_must_be_somewhat_plausible_http()
        {
            var expectedException = false;
            try
            {
                var profile = new ApiExportProfile(name: "Name", postUrl: "nonsenseString!/withoutProtocol");
            }
            catch (ArgumentException)
            {
                expectedException = true;
            }
            Assert.True(expectedException, "creating an api export profile with no valid http url did not raise the expected exception");
        }

        [Fact]
        public void StartTimeToken_must_not_equal_endTimeToken()
        {
            var expectedException = false;
            try
            {
                var profile = new ApiExportProfile(
                    name: "Name",
                    postUrl: "https://posturl",
                    startTimeToken: "_START_",
                    endTimeToken: "_START_",
                    jobNameJsonTemplates: new() { { "JobName", "_START_" } });
            }
            catch (ArgumentException)
            {
                expectedException = true;
            }
            Assert.True(expectedException, "creating a profile with startTimeToken equal to endTimeToken did not raise the expected exception");
        }

        [Fact]
        public void Profile_must_have_jobJsonTemplates()
        {
            var expectedException = false;
            try
            {
                var profile = new ApiExportProfile(
                    name: "Name",
                    postUrl: "https://posturl",
                    startTimeToken: "_START_",
                    endTimeToken: "_END_");
            }
            catch (ArgumentException)
            {
                expectedException = true;
            }
            Assert.True(expectedException, "creating a profile without any jobJsonTemplates did not raise the expected exception");
        }

        [Fact]
        public void Profiles_jobJsonTemplates_must_contain_start_and_endtime_token()
        {
            var expectedException = false;
            try
            {
                var profile = new ApiExportProfile(
                    name: "Name",
                    postUrl: "https://posturl",
                    startTimeToken: "_START_",
                    endTimeToken: "_END_",
                    jobNameJsonTemplates: new() { { "JobName", "emptytemplate" } });
            }
            catch (ArgumentException)
            {
                expectedException = true;
            }
            Assert.True(expectedException, "creating a profile's jobJsonTemplates not containing tokens did not raise the expected exception");

            expectedException = false;
            try
            {
                var profile = new ApiExportProfile(
                    name: "Name",
                    postUrl: "https://posturl",
                    startTimeToken: "_START_",
                    endTimeToken: "_END_",
                    jobNameJsonTemplates: new() { { "JobName", "_START_" } });
            }
            catch (ArgumentException)
            {
                expectedException = true;
            }
            Assert.True(expectedException, "creating a profile's jobJsonTemplates not containing end time token did not raise the expected exception");

            expectedException = false;
            try
            {
                var profile = new ApiExportProfile(
                    name: "Name",
                    postUrl: "https://posturl",
                    startTimeToken: "_START_",
                    endTimeToken: "_END_",
                    jobNameJsonTemplates: new() { { "JobName", "_END_" } });
            }
            catch (ArgumentException)
            {
                expectedException = true;
            }
            Assert.True(expectedException, "creating a profile's jobJsonTemplates not containing start time token did not raise the expected exception");
        }

        [Fact]
        public void JobJsonTemplates_must_contain_startTimeToken_and_endTimeToken()
        {
            var expectedException = false;
            try
            {
                var profile = new ApiExportProfile(
                    name: "Name",
                    postUrl: "https://posturl",
                    startTimeToken: "_START_",
                    endTimeToken: "_END_",
                    jobNameJsonTemplates: new() { { "JobName", "emptytemplate" } });
            }
            catch (ArgumentException)
            {
                expectedException = true;
            }
            Assert.True(expectedException, "creating a profile with template without start and endtime token did not raise the expected exception");

            expectedException = false;
            try
            {
                var profile = new ApiExportProfile(
                    name: "Name",
                    postUrl: "https://posturl",
                    startTimeToken: "_START_",
                    endTimeToken: "_END_",
                    jobNameJsonTemplates: new() { { "JobName", "_END_" } });
            }
            catch (ArgumentException)
            {
                expectedException = true;
            }
            Assert.True(expectedException, "creating a profile with template without starttime token did not raise the expected exception");

            expectedException = false;
            try
            {
                var profile = new ApiExportProfile(
                    name: "Name",
                    postUrl: "https://posturl",
                    startTimeToken: "_START_",
                    endTimeToken: "_END_",
                    jobNameJsonTemplates: new() { { "JobName", "_START_" } });
            }
            catch (ArgumentException)
            {
                expectedException = true;
            }
            Assert.True(expectedException, "creating a profile with template without endtime token did not raise the expected exception");
        }

        [Fact]
        public void Start_and_endtime_tokens_must_not_overlap()
        {
            var expectedException = false;
            try
            {
                var profile = new ApiExportProfile(
                    name: "Name",
                    postUrl: "https://posturl",
                    startTimeToken: "_START_",
                    endTimeToken: "_END_",
                    jobNameJsonTemplates: new() { { "JobName", "_START_END_" } });
            }
            catch (ArgumentException)
            {
                expectedException = true;
            }
            Assert.True(expectedException, "creating a profile with template wit overlapping start and endtime token did not raise the expected exception");

            expectedException = false;
            try
            {
                var profile = new ApiExportProfile(
                    name: "Name",
                    postUrl: "https://posturl",
                    startTimeToken: "_START_",
                    endTimeToken: "_END_",
                    jobNameJsonTemplates: new() { { "JobName", "_END_START_" } });
            }
            catch (ArgumentException)
            {
                expectedException = true;
            }
            Assert.True(expectedException, "creating a profile with template wit overlapping start and endtime token did not raise the expected exception");
        }
    }
}