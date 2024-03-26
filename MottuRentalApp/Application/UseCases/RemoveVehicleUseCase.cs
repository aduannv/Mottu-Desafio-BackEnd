using MottuRentalApp.Application.Ports;
using MottuRentalApp.Application.Facades;
using MottuRentalApp.Application.Exceptions;

namespace MottuRentalApp.Application.UseCases
{
  public class RemoveVehicleUseCase
  {
    private readonly IVehiclesPort _vehiclesPort;
    private readonly RentalVehiclesFacade _rentalVehiclesFacade;
    private const string FEATURE_NAME = "REMOVE_VEHICLE";

    public RemoveVehicleUseCase(IVehiclesPort vehiclesPort, RentalVehiclesFacade rentalVehiclesFacade)
    {
      this._vehiclesPort = vehiclesPort;
      this._rentalVehiclesFacade = rentalVehiclesFacade;
    }

    public void Execute(string licensePlate)
    {
      try
      {
        if (this._rentalVehiclesFacade.IsVehicleAvailable(licensePlate)) {
          this._vehiclesPort.removeVehicle(licensePlate);
        } else {
          throw new UnavailableVehicleException("VEHICLE_NOT_AVAILABLE", FEATURE_NAME);
        }
      }
      catch(Exception exc)
      {
        throw new UnavailableVehicleException(exc.Message, FEATURE_NAME);
      }
    }
  }
}
