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
                var profile = new ApiExportProfile(name: "Name", "nonsenseString!/withoutProtocol");
            }
            catch (ArgumentException)
            {
                expectedException = true;
            }
            Assert.True(expectedException, "creating an api export profile with no valid http url did not raise the expected exception");
        }
    }
}