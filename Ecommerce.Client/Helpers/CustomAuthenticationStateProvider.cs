using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.DTO.Response;
using Shared.DTO.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Ecommerce.Client.Helpers
{
    public class CustomAuthenticationStateProvider(LocalStorageService localStorageService) : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal anonymous = new(new ClaimsIdentity());
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //Lấy token từ LocalStorage:
            var stringToken = await localStorageService.GetToken();
            if (string.IsNullOrEmpty(stringToken)) return await Task.FromResult(new AuthenticationState(anonymous));//Nếu null or rỗng ko có token lưu trữ, nếu ko cótoken, nghĩa là người dùng chưa đăng nhập hoặc token đã bị xóa

            var deserializeToken = Serializations.DeserializeJsonString<AuthResponseDto>(stringToken);
            if (deserializeToken == null) return await Task.FromResult(new AuthenticationState(anonymous));

            var getUserClaims = DecryptToken(deserializeToken.Token!);
            if (getUserClaims == null) return await Task.FromResult(new AuthenticationState(anonymous));

            var claimsPrincipal = SetClaimPrincipal(getUserClaims);
            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }

        public async Task UpdateAuthenticationState(AuthResponseDto userSession)
        {
            var claimsPrincipal = new ClaimsPrincipal();
            if (userSession.Token != null)
            {
                var serializeSession = Serializations.SerializeObj(userSession);//Serialize đối tượng userSession thành chuỗi JSON.
                await localStorageService.SetToken(serializeSession);//Lưu trữ chuỗi JSON này vào LocalStorage.

                var getUserClaims = DecryptToken(userSession.Token!);//Giải mã token để lấy thông tin người dùng.
                claimsPrincipal = SetClaimPrincipal(getUserClaims);//Tạo ClaimsPrincipal từ các thông tin người dùng giải mã được.
            }
            else
            {
                await localStorageService.RemoveToken();//Nếu không có token hoặc refresh token, xóa token khỏi LocalStorage.
            }
            //Gửi thông báo rằng trạng thái xác thực đã thay đổi, cập nhật trạng thái xác thực với claimsPrincipal mới.
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public static ClaimsPrincipal SetClaimPrincipal(CustomUserClaims claims) //Kiểm tra thông tin người dùng Nếu email của người dùng là null, trả về một ClaimsPrincipal mặc định
        {
            if (claims.Email is null) return new ClaimsPrincipal();
            return new ClaimsPrincipal(new ClaimsIdentity(
                new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, claims.Id!),
                    new(ClaimTypes.Name, claims.Name!),
                    new(ClaimTypes.Email, claims.Email!),
                    new(ClaimTypes.Role, claims.Role!),
                }, "JwtAuth"));
            //Tạo ClaimsPrincipal: Tạo một ClaimsPrincipal mới với danh sách các Claim (tên định danh, tên, email, vai trò) và phương thức xác thực là "JwtAuth".
        }

        private static CustomUserClaims DecryptToken(string jwtToken)
        {
            if (string.IsNullOrEmpty(jwtToken)) return new CustomUserClaims();

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            var userId = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var name = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            var email = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            var role = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            return new CustomUserClaims(userId!.Value, name!.Value, email!.Value, role!.Value);
        }
    }
}
