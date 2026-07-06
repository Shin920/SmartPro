# SmartPro

ASP.NET WebForms 기반의 SmartPro 업무 시스템입니다.  
DevExpress WebForm 컴포넌트를 사용하며, 웹 UI(`SMART`)와 업무별 서비스/데이터 접근 DLL(`SMARTSVC`), 공통 라이브러리(`CFL`)로 구성된 ERP/MES 계열 솔루션입니다.

## 프로젝트 개요

SmartPro는 로그인, 메인 포털, 공통 마스터, 게시판, 결재/워크플로우, 리포트, 모바일 화면 등을 포함하는 WebForms 기반 업무 시스템입니다.  
각 업무 영역은 `DA(Data Access)` / `DS(Data Service)` 형태의 클래스 라이브러리로 분리되어 있으며, 웹 프로젝트는 해당 DLL을 참조하여 화면과 업무 로직을 구성합니다.

## 주요 기술 스택

| 구분 | 내용 |
|---|---|
| Framework | ASP.NET WebForms, .NET Framework 4.6.1 |
| Language | C# |
| UI Component | DevExpress Web v20.2 계열 |
| DB Access | System.Data.SqlClient 기반 MSSQL 연동 |
| Package | Newtonsoft.Json 13.0.1, System.Net.Http 4.3.4 |
| IDE | Visual Studio 2017 이상 권장 |
| Web Server | IIS / Local IIS |

## 솔루션 구성

```text
SmartPro-main/
├─ Smart 21.0_SmartPro.sln        # 메인 솔루션
├─ SMART/                         # ASP.NET WebForms 웹사이트
│  ├─ Login.aspx                   # 로그인 화면
│  ├─ glb/                         # 메인/포털/게시판/일정 등 글로벌 화면
│  ├─ Common/                      # 공통 폼, 팝업, 결재, 리포트 출력
│  ├─ Mobile/                      # 모바일 WebForms 화면
│  ├─ Contents/                    # CSS, JS, 이미지, SmartEditor 리소스
│  ├─ App_Code/                    # 웹서비스/공통 코드
│  └─ web.config                   # 웹 설정 및 DevExpress/IIS 설정
├─ SMARTSVC/                       # 업무별 서비스/데이터 접근 프로젝트
│  ├─ CommonProject/               # 공통 코드, 로그인, 모듈, 결재, 게시판
│  ├─ LOGProject/                  # 물류/생산/구매/품질 등 업무 모듈
│  ├─ FINProject/                  # 회계/재무 계열 모듈
│  ├─ HRMProject/                  # 인사 계열 모듈
│  ├─ MNGProject/                  # 관리 계열 모듈
│  ├─ MobileProject/               # 모바일 비즈니스/코어 모듈
│  ├─ ReportProject/               # 리포트 모듈
│  └─ CGWProject/                  # 그룹웨어/외부 연동 프로젝트
├─ CFL/CommonFunctionLibrary/      # 공통 함수/보안/데이터/페이지 베이스 라이브러리
├─ WorkFlow/                       # 워크플로우 컨트롤 라이브러리
├─ SMART.ES.DA/                    # ES 업무 데이터 접근 프로젝트
└─ SMART.ES.DS/                    # ES 업무 서비스 프로젝트
```

## 주요 기능

- 사용자 로그인 및 세션 기반 업무 화면 접근
- 메인 포털, 사용자 정보, 공지사항, 일정, 결재 현황 표시
- 공통 마스터 관리
  - 부서, 거래처, 계정, 창고, 품목/사업장, 사용자 그룹 등
- 공통 팝업 및 채번 팝업 제공
- 워크플로우/전자결재 화면 및 결재 문서 관리
- DevExpress 기반 그리드, 차트, 리포트, 스케줄러, 대시보드 컴포넌트 사용
- 게시판/공지사항 관리
- 출력/리포트 화면 제공
- 모바일 로그인 및 모바일 업무 화면 제공
- 그룹웨어/외부 웹서비스 연동 구조 포함

## 업무 모듈 구조

`SMARTSVC` 하위 프로젝트는 대체로 다음과 같은 패턴으로 구성되어 있습니다.

| 접미사 | 의미 |
|---|---|
| `.DA` | Data Access 계층. DB 조회/저장/삭제 등 데이터 처리 담당 |
| `.DS` | Data Service 계층. 화면 또는 서비스에서 호출하는 업무 로직 담당 |

주요 모듈 예시는 다음과 같습니다.

| 영역 | 프로젝트 |
|---|---|
| 공통 | `SMART.COMMON.DA`, `SMART.COMMON.DS` |
| 물류/생산/구매/품질 | `SMART.SD`, `SMART.PO`, `SMART.PP`, `SMART.QM`, `SMART.IM`, `SMART.NP`, `SMART.PM`, `SMART.OS`, `SMART.EX` |
| 재무/회계 | `SMART.AA`, `SMART.CO`, `SMART.GL`, `SMART.TR` |
| 인사 | `SMART.HR` |
| 관리 | `SMART.OM` |
| 모바일 | `MobileBiz`, `MobileCore` |
| 리포트 | `SMART.REPORT` |
| 외부 연동 | `CGWProject` |

## 실행 및 개발 환경 설정

### 1. 필수 설치 항목

- Visual Studio 2017 이상
- .NET Framework 4.6.1 Developer Pack
- IIS 또는 Local IIS
- DevExpress v20.2.6 WebForms 관련 컴포넌트
- MSSQL Server 접속 환경

### 2. 솔루션 열기

Visual Studio에서 아래 솔루션을 엽니다.

```text
Smart 21.0_SmartPro.sln
```

해당 솔루션은 Web Site 프로젝트를 Local IIS의 `/SmartPro` 가상 디렉터리 기준으로 사용하도록 설정되어 있습니다.

### 3. IIS 가상 디렉터리 설정

웹 프로젝트 경로를 IIS에 연결합니다.

```text
가상 디렉터리: /SmartPro
물리 경로: SMART/
응용 프로그램 풀: .NET Framework v4.x, Integrated Pipeline
```

### 4. web.config 설정

`SMART/web.config`에서 환경별 설정을 변경합니다.

```xml
<appSettings>
  <add key="QueryLogPath" value="C:\SmartLog\QueryLog" />
  <add key="UploadPath" value="C:\SMART\FileUpload" />
  <add key="LocalPath" value="C:\Smart 21.0_SmartPro" />
  <add key="ResourceRoot" value="C:\Smart 21.0_SmartPro\SMART" />
</appSettings>

<connectionStrings>
  <add name="ScheduleConnectionString"
       connectionString="Data Source=DB_SERVER;User ID=DB_USER;Password=DB_PASSWORD;Initial Catalog=DB_NAME;Connect Timeout=3600"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

> 실제 DB 서버, 계정, 비밀번호는 배포 환경에 맞게 별도 관리하는 것을 권장합니다.

### 5. NuGet 패키지 복원

`SMART/packages.config` 기준으로 다음 패키지가 사용됩니다.

```text
Newtonsoft.Json 13.0.1
System.Net.Http 4.3.4
```

Visual Studio에서 `Restore NuGet Packages`를 실행합니다.

### 6. DevExpress 참조 확인

`web.config`에 DevExpress v20.2.6 계열 Assembly가 등록되어 있습니다.

대표 참조:

```text
DevExpress.Web.v20.2
DevExpress.XtraCharts.v20.2
DevExpress.XtraReports.v20.2
DevExpress.Dashboard.v20.2
DevExpress.Web.ASPxTreeList.v20.2
DevExpress.Web.ASPxScheduler.v20.2
```

빌드 오류가 발생할 경우 설치된 DevExpress 버전과 `web.config`, `licenses.licx`, 프로젝트 참조 버전이 일치하는지 확인해야 합니다.

## 빌드 순서 참고

일반적으로 다음 순서로 빌드하는 것이 안전합니다.

1. `CFL/CommonFunctionLibrary`
2. `WorkFlow`
3. `SMARTSVC` 하위 공통/업무 모듈 DLL
4. `SMART` WebForms 사이트

Visual Studio 솔루션의 프로젝트 참조가 정상 구성되어 있다면 전체 빌드로 처리할 수 있습니다.

## 배포 참고

- IIS에 `/SmartPro` 응용 프로그램을 생성합니다.
- `SMART` 폴더를 웹 루트로 배포합니다.
- 업무 DLL은 `SMART/bin`에 배포되어야 합니다.
- DevExpress Runtime/Assembly가 서버에 준비되어 있어야 합니다.
- `UploadPath`, `QueryLogPath` 등 파일 저장 경로는 IIS AppPool 계정에 쓰기 권한이 필요합니다.
- `web.config`의 DB 접속정보와 외부 서비스 URL은 운영 환경에 맞게 변경해야 합니다.

## 주의 사항

- 저장소 내 `web.config`에는 운영/개발 환경 정보가 포함될 수 있으므로 공개 저장소 업로드 전 민감정보 제거가 필요합니다.
- DevExpress 라이선스 파일(`licenses.licx`)이 포함되어 있으므로 라이선스 정책을 확인해야 합니다.
- `web.config` 일부 Handler 설정에 DevExpress 버전 표기가 혼재되어 있을 수 있으므로, 실제 설치 버전 기준으로 정리하는 것이 좋습니다.
- `SMART/Files`, `UserImages` 등 업로드/첨부성 파일은 운영 데이터가 포함될 수 있어 배포/공개 전 검토가 필요합니다.

## 한 줄 요약

SmartPro는 ASP.NET WebForms와 DevExpress를 기반으로 구축된 ERP/MES 성격의 업무 시스템이며, 웹 화면과 업무별 DA/DS 클래스 라이브러리를 분리한 구조로 구성되어 있습니다.
