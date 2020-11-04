using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiParquimetros.Models
{
    public class Movimientos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[(DeleteBehavior.Restrict)]
        [Column(Order = 1, TypeName = "integer")]
        public int id { get; set; }

        [StringLength(maximumLength: 225)]
        public string created_by { get; set; }

        public System.Nullable<DateTime> created_date { get; set; }

        [StringLength(maximumLength: 225)]
        public string last_modified_by { get; set; }

        public DateTime last_modified_date { get; set; }
        [DefaultValue(true)]
        public Boolean bit_status { get; set; }

        public string str_placa { get; set; }
        public string str_latitud { get; set; }
        public string str_longitud { get; set; }

        public Boolean boolean_auto_recarga { get; set; }

        public Boolean boolean_multa { get; set; }

        public DateTime dt_hora_inicio { get; set; }


        public System.Nullable<DateTime> dtm_fecha_insercion_descuento { get; set; }

        public System.Nullable<DateTime> dtm_fecha_descuento { get; set; }

        public DateTime dtm_hora_fin { get; set; }
        //Este campo no estaba
        public int int_tiempo_comprado { get; set; }
        //***1
        public int int_tiempo { get; set; }

        public Double flt_moneda_saldo_previo_descuento { get; set; }
        //***2
        public Double flt_monto { get; set; }
    
        public Double flt_porcentaje_comision { get; set; }
        //***3
        public Double flt_monto_porcentaje { get; set; }
        //***4
        public Double flt_total_con_comision { get; set; }
        public string str_nombre_concesion { get; set; }
        public Double flt_saldo_anterior { get; set; }

        public int int_tiempo_devuelto { get; set; }
        public Double flt_monto_devolucion { get; set; }
        public Double flt_monto_porc_devolucion { get; set; }
        public Double flt_total_dev_con_comision { get; set; }
        public Double flt_monto_real { get; set; }

        public System.Nullable<Double> flt_saldo_previo_descuento { get; set; }

        public System.Nullable<Double> flt_valor_descuento { get; set; }

        public System.Nullable<Double> flt_valor_devuelto { get; set; }

        public System.Nullable<Double> flt_valor_final_descuento { get; set; }

        [StringLength(maximumLength: 50)]
        public string str_cambio_descuento { get; set; }

        [StringLength(maximumLength: 50)]
        public string str_codigo_autorizacion { get; set; }

        [StringLength(maximumLength: 50)]
        public string str_codigo_transaccion { get; set; }

        [StringLength(maximumLength: 200)]
        public string str_comentarios { get; set; }

        [StringLength(maximumLength: 50)]
        public string str_hash_tarjeta { get; set; }

        [StringLength(maximumLength: 50)]
        public string str_instalacion { get; set; }

        [StringLength(maximumLength: 50)]
        public string str_instalacion_abrv { get; set; }

        [StringLength(maximumLength: 50)]
        public string str_moneda_valor_descuento { get; set; }

        [StringLength(maximumLength: 50)]
        public string str_referencia_operacion { get; set; }

        [StringLength(maximumLength: 50)]
        public string str_so { get; set; }

        [StringLength(maximumLength: 50)]
        public string str_tipo { get; set; }

        [StringLength(maximumLength: 50)]
        public string str_versionapp { get; set; }

        [ForeignKey("tbespacios")]

        public System.Nullable<int> int_id_espacio { get; set; }

        [ForeignKey("tbsaldo")]
        public System.Nullable<int> int_id_saldo_id { get; set; }
        //[Required]
        [ForeignKey("NetUsers")]
        public string int_id_usuario_id { get; set; }
        [Required]
        [ForeignKey("tbvehiculos")]
        public int int_id_vehiculo_id { get; set; }

        public Espacios tbespacios { get; set; }
        public Saldos tbsaldo { get; set; }
        ////esta linea se comento para que en las consultas no llevara la lista info del usuario revisar si no afecta la llave foranea int_id_usuario_id
        //public ApplicationUser NetUsers { get; set; }

        public Vehiculos tbvehiculos { get; set; }

        [ForeignKey("tbconcesiones")]
        public int intidconcesion_id { get; set; }
        public Concesiones tbconcesiones { get; set; }

        public System.Nullable<int> int_id_multa { get; set; }

        public string InsDescription { get; set; }
        public string InsShortdesc { get; set; }
        public Double BalanceBefore { get; set; }
        public int TicketNumber { get; set; }
        public string Sector { get; set; }
        public string Tariff { get; set; }
        public Double DiscountAmountCurrencyId { get; set; }
        public Double DiscountBalanceCurrencyId { get; set; }
        public Double DiscountBalanceBefore { get; set; }
        public int ServiceChargeTypeId { get; set; }
        public string CardReference { get; set; }
        public string CardScheme { get; set; }
        public string MaskedCardNumber { get; set; }
        public DateTime CardExpirationDate { get; set; }
        public int ExternalId1 { get; set; }
        public int ExternalId2 { get; set; }
        public int ExternalId3 { get; set; }
        public Double PercVat1 { get; set; }
        public Double PercVat2 { get; set; }
        public Double PartialVat1 { get; set; }
        public Double PercFee { get; set; }
        public Double PercFeeTopped { get; set; }
        public Double PartialPercFee { get; set; }
        public Double FixedFee { get; set; }
        public Double PartialFixedFee { get; set; }
        public Double TotalAmount { get; set; }
        public Double CuspmrPagateliaNewBalance { get; set; }
        public string CuspmrType { get; set; }
        public Boolean ShopkeeperOp { get; set; }
        public Double ShopkeeperAmount { get; set; }
        public Double ShopkeeperProfit { get; set; }
      
        public string Plate2 { get; set; }
        public string Plate3 { get; set; }
        public string Plate4 { get; set; }
        public string Plate5 { get; set; }
        public string Plate6{ get; set; }
        public string Plate7 { get; set; }
        public string Plate8 { get; set; }
        public string Plate9 { get; set; }
        public string Plate10 { get; set; }
        public Boolean PermitAutoRenew { get; set; }
        public Boolean PermitExpiration { get; set; }
        public Boolean TransStatus { get; set; }
        public Double RefundAmount { get; set; }
        public Double valor_sin_bonificar { get; set; }
        public Double bonificacion { get; set; }
        public string tipo_vehiculo { get; set; }
      
















    }
    public class MovimientosActivosInactivos {
        public int id { get; set; }
        public string created_by { get; set; }

        public System.Nullable<DateTime> created_date { get; set; }

        public string last_modified_by { get; set; }

        public DateTime last_modified_date { get; set; }

        public Boolean bit_status { get; set; }

        public string str_placa { get; set; }

        public Boolean boolean_auto_recarga { get; set; }

        public Boolean boolean_multa { get; set; }

        public DateTime dt_hora_inicio { get; set; }
        public string str_latitud { get; set; }
        public string str_longitud { get; set; }


        public System.Nullable<DateTime> dtm_fecha_insercion_descuento { get; set; }

        public System.Nullable<DateTime> dtm_fecha_descuento { get; set; }

        public DateTime dtm_hora_fin { get; set; }

        public int int_tiempo { get; set; }

        public Double flt_moneda_saldo_previo_descuento { get; set; }

        public Double flt_monto { get; set; }

        public System.Nullable<Double> flt_saldo_previo_descuento { get; set; }

        public System.Nullable<Double> flt_valor_descuento { get; set; }

        public System.Nullable<Double> flt_valor_devuelto { get; set; }

        public System.Nullable<Double> flt_valor_final_descuento { get; set; }

        public string str_cambio_descuento { get; set; }
        public string str_codigo_autorizacion { get; set; }

        public string str_codigo_transaccion { get; set; }
        public string str_comentarios { get; set; }
        public string str_hash_tarjeta { get; set; }

        public string str_instalacion { get; set; }

        public string str_instalacion_abrv { get; set; }

        public string str_moneda_valor_descuento { get; set; }
        public string str_referencia_operacion { get; set; }
        public string str_so { get; set; }

        public string str_tipo { get; set; }

        public string str_versionapp { get; set; }
        public string str_nombre_zona { get; set; }

        public int int_id_espacio { get; set; }

        public int int_id_saldo_id { get; set; }

        public string int_id_usuario_id { get; set; }

        public int int_id_vehiculo_id { get; set; }

        public int intidconcesion_id { get; set; }

        public System.Nullable<int> int_id_multa { get; set; }

    }

    public class MovimientosXId
    {
        public int id { get; set; }
        public string created_by { get; set; }

        public System.Nullable<DateTime> created_date { get; set; }

        public string last_modified_by { get; set; }

        public DateTime last_modified_date { get; set; }

        public Boolean bit_status { get; set; }

        public string str_placa { get; set; }

        public Boolean boolean_auto_recarga { get; set; }

        public Boolean boolean_multa { get; set; }

        public DateTime dt_hora_inicio { get; set; }


        public System.Nullable<DateTime> dtm_fecha_insercion_descuento { get; set; }

        public System.Nullable<DateTime> dtm_fecha_descuento { get; set; }

        public DateTime dtm_hora_fin { get; set; }

        public int int_tiempo { get; set; }

        public Double flt_moneda_saldo_previo_descuento { get; set; }

        public Double flt_monto { get; set; }

        public System.Nullable<Double> flt_saldo_previo_descuento { get; set; }

        public System.Nullable<Double> flt_valor_descuento { get; set; }

        public System.Nullable<Double> flt_valor_devuelto { get; set; }

        public System.Nullable<Double> flt_valor_final_descuento { get; set; }

        public string str_cambio_descuento { get; set; }
        public string str_codigo_autorizacion { get; set; }

        public string str_codigo_transaccion { get; set; }
        public string str_comentarios { get; set; }
        public string str_hash_tarjeta { get; set; }

        public string str_instalacion { get; set; }

        public string str_instalacion_abrv { get; set; }

        public string str_moneda_valor_descuento { get; set; }
        public string str_referencia_operacion { get; set; }
        public string str_so { get; set; }

        public string str_tipo { get; set; }

        public string str_versionapp { get; set; }

        public string str_latitud { get; set; }
        public string str_longitud { get; set; }

        public int int_id_zona { get; set; }
        public string str_nombre_zona { get; set; }

        public int int_id_espacio { get; set; }

        public int int_id_saldo_id { get; set; }

        public string int_id_usuario_id { get; set; }

        public int int_id_vehiculo_id { get; set; }

        public int intidconcesion_id { get; set; }

        public System.Nullable<int> int_id_multa { get; set; }

    }

    public class IngresosXDiaIOS
    {
        public int intTransIos { get; set; }
        public Double dblingresosIOS { get; set; }
    }
    public class IngresosXDiaAndroid
    {
        public int intTransAndroid { get; set; }
        public Double dblingresosAndroid { get; set; }
    }

    public class DatosDashboard
    {
        public Double dblSemanalAndriod { get; set; }
        public DataTable dtSemanaAndroid { get; set; }
        public Double dblSemanalIOS { get; set; }
        public DataTable dtSemanaIOS { get; set; }

        public Double dblIngresosDiaAndriod { get; set; }
        public Double int_porcentaje_andriod { get; set; }
        public Double dbl_total_ant_andriod { get; set; }

        public Double dblIngresosDiaIOS { get; set; }
        public Double int_porcentaje_IOS { get; set; }
        public Double dbl_total_ant_IOS { get; set; }

        public Double dbl_total_del_dia { get; set; }
        public Double int_porcentaje_total { get; set; }
        public Double dbl_total_dia_ant { get; set; }


        public Double dbl_total_ac_semana { get; set; }
        public Double int_porcentaje_semana { get; set; }
        public Double dbl_total_ant_semana { get; set; }
        //public DataTable CompIngresosAndroidSem { get; set; }
        //public DataTable CompIngresosIOSSem{ get; set; }


    }

    public class CompIngresos {
        public DataTable dtFechas{get; set;}
        public Double dblMonto { get; set; }
        public string strPlataforma { get; set; }

    }

    public class ResumenIngresosMensual
    {
        public DataTable dtFechas { get; set; }
        public Double dblMontoMensual { get; set; }
        public Double dblPorcentajeIngresos  { get; set; }
        public int intPorcentajeTransacciones  { get; set; }

    }

    public class IngresosMensuales
    {
        public double SaldoUsuarioMesAnterior { get; set; }
        public double RecargaUsuario { get; set; }
        public double ComisionRecarga { get; set; }
        public double TotalCobradoRecarga { get; set; }
        public double SaldoUsuarioMes { get; set; }
        public double VentaConcesion { get; set; }
        public double Comision { get; set; }
        public double TotalCobradoCompra { get; set; }
        public double SaldoFinalUsuaios { get; set; }
        public double IngresoXComisiones { get; set; }


    }
}
