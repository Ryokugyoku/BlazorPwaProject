# 認証アーキテクチャについて
# Identityフレームワーク
 ASP.NETフレームワークで提供している、デフォルトの認証システムは　Identityフレームワークという仕組みで動いている。
 上記のフレームワークを利用して、ユーザの認証と認可を行うことができる。

## 認証と認可の違い
### 認証
 利用者本人の確認

 ### 認可
  利用者に対して保有している権限を与える処理

# 認証用テーブルについて
## AspNetRoles
 テーブル名の通りロールを管理する

## AspNetUsersRoles
 AspNetRolesとAspNetUserを紐づけるためのテーブル

## AspNetUsers
 ユーザの登録された情報が格納されている

## AspNetRoleClaims & AspNetUserClaims
 ユーザに対して固有のサブロールを設定することができる。
 AspNetRolesだけを利用してロールを設定することができるが、AspNetRolesとAspNetRoleClaimsを紐づけることで
 そのロールに属するアカウントすべては、対象の権原を持つことができる。

 適切なイメージをあげるなら、AspNetRolesで一般ロールを持っているユーザＡが一般ロールのユーザＢのデータの閲覧権を
 クレームAspNetUserClaimsというテーブルでユーザとクレームを紐づけることで対象のアクセス権を渡すことができる。

 仮に特定のユーザグループに対してＢが所有しているデータの閲覧権を与える場合は、AspNetUserClaimsテーブルとクレームを紐づける
 ことで、特定のグループに対して自身が保有している権原を与えることができる。

 # 認証情報を設定方法

 ## WebAssemblyでAuthenbticationの有効化
 ```
 	builder.Services.AddHttpClient("GeneralSecureApp.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
	.AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

 ```
 ## Server側の認証情報の設定、ロールを扱えるように下記の設定をする必要がある
 ```
 	  //ロールを追加
	  builder.Services.AddDefaultIdentity<ApplicationUser>()
	       .AddRoles<IdentityRole>()
	      .AddEntityFrameworkStores<ApplicationDbContext>();
	  // Client側でロールを管理できるようにするための設定
	  builder.Services.AddIdentityServer()
	      .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options => {
	          options.IdentityResources["openid"].UserClaims.Add("role");
	          options.ApiResources.Single().UserClaims.Add("role");
	      });
	  System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler
	  .DefaultInboundClaimTypeMap.Remove("role");

 ```

 # ローカライズ
 ## クライアントサイド
  IStringLocalizerをNugetでインストールして利用する。
  resxファイルは Sharedファイルに格納することで、サーバサイドでも共通して利用することができる。

  .razorファイルで利用する際は下記のようにコードを記述する必要がある
  ① 下記のようにIStringLocalizerを宣言する
 ```
	@inject IStringLocalizer<Lang> LoText
 ```
  ②宣言した変数を元に対象文字列を呼び出す
   ※ resxファイルの project_titleを呼び出すとき
   ```
   LoText["project_title"] 
   ```
  