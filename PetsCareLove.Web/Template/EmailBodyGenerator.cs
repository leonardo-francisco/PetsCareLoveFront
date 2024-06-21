namespace PetsCareLove.Web.Template
{
    public static class EmailBodyGenerator
    {
        public static string GenerateConfettiEmailBody()
        {
            return @"
                  <!DOCTYPE html>
                  <html lang='en'>
                  <head>
                      <meta charset='UTF-8'>
                      <title>Parabéns</title>
                      <style>
                           body {
  background: #feffef;
  font-family: Helvetica, sans-serif;
}

@keyframes bang {
  from {
    transform: translate3d(0, 0, 0);
    opacity: 1;
  }
}
.hoverme {
  width: 240px;
  margin: 150px auto 0;
  text-align: center;
  padding: 10px 0;
  cursor: pointer;
  position: relative;
}
.hoverme span {
  color: #333;
  font-size: .9em;
}
.hoverme i {
  position: absolute;
  display: block;
  left: 50%;
  top: 0;
  width: 3px;
  height: 5px;
  background: red;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(1) {
  transform: translate3d(-54px, -72px, 0) rotate(82deg);
  background: #ff0037;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(2) {
  transform: translate3d(25px, -55px, 0) rotate(200deg);
  background: #ff0077;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(3) {
  transform: translate3d(61px, -76px, 0) rotate(236deg);
  background: #77ff00;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(4) {
  transform: translate3d(-46px, -74px, 0) rotate(153deg);
  background: #00ffa2;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(5) {
  transform: translate3d(39px, -86px, 0) rotate(4deg);
  background: #00ffd5;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(6) {
  transform: translate3d(-36px, -64px, 0) rotate(36deg);
  background: #00aeff;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(7) {
  transform: translate3d(64px, -92px, 0) rotate(179deg);
  background: #00b3ff;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(8) {
  transform: translate3d(61px, -73px, 0) rotate(252deg);
  background: #22ff00;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(9) {
  transform: translate3d(20px, -69px, 0) rotate(116deg);
  background: #d0ff00;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(10) {
  transform: translate3d(-30px, -52px, 0) rotate(344deg);
  background: #e100ff;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(11) {
  transform: translate3d(71px, -86px, 0) rotate(287deg);
  background: #3cff00;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(12) {
  transform: translate3d(50px, -55px, 0) rotate(57deg);
  background: #00ff7b;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(13) {
  transform: translate3d(-53px, -70px, 0) rotate(274deg);
  background: #00aeff;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(14) {
  transform: translate3d(-90px, -71px, 0) rotate(96deg);
  background: #ff6200;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(15) {
  transform: translate3d(23px, -66px, 0) rotate(91deg);
  background: #ff0026;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(16) {
  transform: translate3d(-26px, -54px, 0) rotate(154deg);
  background: #0051ff;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(17) {
  transform: translate3d(64px, -58px, 0) rotate(255deg);
  background: #0040ff;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(18) {
  transform: translate3d(64px, -95px, 0) rotate(79deg);
  background: #9dff00;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(19) {
  transform: translate3d(45px, -50px, 0) rotate(343deg);
  background: #00fbff;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(20) {
  transform: translate3d(-41px, -63px, 0) rotate(3deg);
  background: #002bff;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(21) {
  transform: translate3d(-18px, -77px, 0) rotate(225deg);
  background: #a200ff;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(22) {
  transform: translate3d(0px, -71px, 0) rotate(185deg);
  background: #ff1e00;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(23) {
  transform: translate3d(1px, -81px, 0) rotate(144deg);
  background: #00ff91;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(24) {
  transform: translate3d(-28px, -88px, 0) rotate(337deg);
  background: #ff0095;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(25) {
  transform: translate3d(71px, -66px, 0) rotate(326deg);
  background: #00f2ff;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(26) {
  transform: translate3d(-82px, -72px, 0) rotate(91deg);
  background: #5100ff;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(27) {
  transform: translate3d(-32px, -66px, 0) rotate(6deg);
  background: #ffb700;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(28) {
  transform: translate3d(-84px, -60px, 0) rotate(34deg);
  background: #00ff66;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(29) {
  transform: translate3d(-11px, -90px, 0) rotate(137deg);
  background: lime;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(30) {
  transform: translate3d(33px, -58px, 0) rotate(118deg);
  background: #ff0080;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(31) {
  transform: translate3d(-9px, -52px, 0) rotate(258deg);
  background: #b300ff;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(32) {
  transform: translate3d(35px, -98px, 0) rotate(149deg);
  background: #e600ff;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(33) {
  transform: translate3d(-81px, -71px, 0) rotate(281deg);
  background: #9dff00;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(34) {
  transform: translate3d(24px, -86px, 0) rotate(170deg);
  background: #ffd900;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(35) {
  transform: translate3d(48px, -68px, 0) rotate(197deg);
  background: #3300ff;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(36) {
  transform: translate3d(10px, -85px, 0) rotate(344deg);
  background: #00ff2f;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(37) {
  transform: translate3d(-82px, -59px, 0) rotate(284deg);
  background: #006fff;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(38) {
  transform: translate3d(84px, -54px, 0) rotate(24deg);
  background: #ff0095;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(39) {
  transform: translate3d(-67px, -97px, 0) rotate(265deg);
  background: #003cff;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(40) {
  transform: translate3d(-98px, -61px, 0) rotate(215deg);
  background: #001eff;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(41) {
  transform: translate3d(-19px, -84px, 0) rotate(261deg);
  background: #00eeff;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(42) {
  transform: translate3d(71px, -53px, 0) rotate(230deg);
  background: #007bff;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(43) {
  transform: translate3d(-86px, -70px, 0) rotate(360deg);
  background: #00fff7;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(44) {
  transform: translate3d(79px, -82px, 0) rotate(119deg);
  background: #ff6f00;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(45) {
  transform: translate3d(-33px, -81px, 0) rotate(156deg);
  background: #ff001e;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(46) {
  transform: translate3d(68px, -90px, 0) rotate(341deg);
  background: #0099ff;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(47) {
  transform: translate3d(29px, -84px, 0) rotate(253deg);
  background: #00ffd5;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(48) {
  transform: translate3d(-50px, -80px, 0) rotate(210deg);
  background: #00fbff;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(49) {
  transform: translate3d(-30px, -74px, 0) rotate(36deg);
  background: cyan;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
.hoverme:hover i:nth-of-type(50) {
  transform: translate3d(-25px, -50px, 0) rotate(155deg);
  background: #007bff;
  animation: bang 700ms ease-out forwards;
  opacity: 0;
}
</style>
</head>
<body>
    <div class='hoverme'>
        <span>
            Parabéns. Cadastro feito com sucesso no Pets Care Love. Seja bem vindo.
        </span>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
        <i></i>
    </div>
</body>
</html>";
        }
    }
}
