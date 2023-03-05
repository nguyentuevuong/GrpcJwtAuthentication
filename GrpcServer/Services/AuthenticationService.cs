using Grpc.Core;

namespace GrpcServer.Services
{
  public class AuthenticationService : Authentication.AuthenticationBase
  {
    public override Task<AuthenticationResponse> Authenticate(AuthenticationRequest request, ServerCallContext context)
    {
      var authenticationResponse = JwtAuthenticationManager.Authenticate(request);

      return authenticationResponse == null
        ? throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid user Credentials"))
        : Task.FromResult(authenticationResponse);
    }
  }
}
